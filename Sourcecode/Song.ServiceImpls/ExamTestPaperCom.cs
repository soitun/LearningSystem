using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;



namespace Song.ServiceImpls
{
    public class ExamTestPaperCom : IExamTestPaper
    {
        #region 相关操作类
        private static OrganizationCom orgCom = new OrganizationCom();
        private static SubjectCom subjectCom = new SubjectCom();
        private static CourseCom courseCom = new CourseCom();
        private static OutlineCom outlineCom = new OutlineCom();
        private static QuestionsCom questionsCom = new QuestionsCom();
        private static AccountsCom accountCom = new AccountsCom();
        private static TestPaperCom testPaperCom = new TestPaperCom();
        #endregion
        #region 试卷管理
        /// <summary>
        /// 添加试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        public long PaperAdd(ExamTestPaper entity)
        {
            if (entity.Etp_Id <= 0) entity.Etp_Id = WeiSha.Core.Request.SnowID();

            if (entity.Org_ID <= 0)
            {
                Song.Entities.Organization org = orgCom.OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
               
            }
            entity.Etp_CrtTime = DateTime.Now;
            //判断是否有简答题，还没有编写
            entity.Etp_IsManual = this.PaperIsManual(entity);
            //
            Gateway.Default.Save<ExamTestPaper>(entity);
            return entity.Etp_Id;
        }
        /// <summary>
        /// 修改试卷
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PaperSave(ExamTestPaper entity)
        {
            entity.Etp_Lasttime = DateTime.Now;
            //判断是否有简答题
            entity.Etp_IsManual = this.PaperIsManual(entity);

            Gateway.Default.Save<ExamTestPaper>(entity);
        }
        /// <summary>
        /// 修改试卷的某些项
        /// </summary>
        /// <param name="id">试卷的id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool PaperUpdate(long id, Field[] fiels, object[] objs)
        {
            Gateway.Default.Update<ExamTestPaper>(fiels, objs, ExamTestPaper._.Etp_Id == id);
            return true;
        }
        /// <summary>
        /// 删除试卷，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PaperDelete(long id)
        {
            return Gateway.Default.Update<ExamTestPaper>(ExamTestPaper._.Etp_IsDeleted, true, ExamTestPaper._.Etp_Id == id && ExamTestPaper._.Etp_IsDeleted == false);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int PaperRecycle(long id)
        {
            return Gateway.Default.Update<ExamTestPaper>(ExamTestPaper._.Etp_IsDeleted, false, ExamTestPaper._.Etp_Id == id && ExamTestPaper._.Etp_IsDeleted == true);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PaperRemove(long id)
        {
            Song.Entities.ExamTestPaper tp = this.PaperSingle(id);
            if (tp == null) return 0;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Etp_Id == id).ToFirst<Examination>();
                    if (exam != null) throw new WeiSha.Core.ExceptionForPrompt($"试卷“{tp.Etp_Name}”已被考试采用，不能删除");

                    tran.Delete<ExamTestPaper>(ExamTestPaper._.Etp_Id == id);
                    //删除图片文件
                    string img = WeiSha.Core.Upload.Get["ExamTestPaper"].Physics + tp.Etp_Logo;
                    if (System.IO.File.Exists(img)) System.IO.File.Delete(img);
                    //删除成绩
                    tran.Delete<ExamResults>(ExamResults._.Etp_Id == id);
                    WeiSha.Core.Upload.Get["ExamTestPaper"].DeleteDirectory(tp.Etp_Id.ToString());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return 1;
        }
        /// <summary>
        /// 获取单一试卷实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public ExamTestPaper PaperSingle(long id)
        {
            return  Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Id == id).ToFirst<ExamTestPaper>();
        }
        /// <summary>
        /// 获取单一试卷实体对象，按试卷名称；
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ExamTestPaper PaperSingle(string name)
        {
            return Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Name == name).ToFirst<ExamTestPaper>();
        }
        /// <summary>
        /// 判断是否有简答题
        /// </summary>
        public bool PaperIsManual(long id)
        {
            ExamTestPaper tp=this.PaperSingle(id);
            if (tp == null) return false;
            return PaperIsManual(tp);
        }
        /// <summary>
        /// 判断是否有简答题
        /// </summary>
        public bool PaperIsManual(ExamTestPaper entity)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(entity.Etp_FromConfig);
            //各题型的占比
            XmlNodeList nodeitems = xmldoc.SelectNodes("/testpaper/questions/ques");
            if (nodeitems != null && nodeitems.Count > 0)
            {
                foreach (XmlNode item in nodeitems)
                {
                    if (item.Attributes["type"]?.Value == "4")
                    {
                        if(Convert.ToInt32(item.Attributes["count"]?.Value) > 0) return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取指定数据的试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="accid">管理员id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperCount(int orgid, int accid, bool? isdeleted, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;          
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(ExamTestPaper._.Etp_Id.Desc).ToList<ExamTestPaper>(count);
        }
        /// <summary>
        /// 获取指定数据的试卷
        /// </summary>
        /// <param name="search"></param>
        /// <param name="accid">管理员id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperCount(int orgid, string search, int accid, bool? isdeleted, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(search)) wc &= ExamTestPaper._.Etp_Name.Contains(search);
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(ExamTestPaper._.Etp_Id.Desc).ToList<ExamTestPaper>(count);
        }
        /// <summary>
        /// 计算有多少个试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public int PaperOfCount(int orgid, int diff, bool? isdeleted, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            return Gateway.Default.Count<ExamTestPaper>(wc);
        }
        /// <summary>
        /// 分页获取试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="accid">管理员id</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="sear">标题检索</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperPager(int orgid, int accid, string sear, bool? isdeleted, int diff, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(sear)) wc &= ExamTestPaper._.Etp_Name.Contains(sear);
            countSum = Gateway.Default.Count<ExamTestPaper>(wc);
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(ExamTestPaper._.Etp_Id.Desc).ToList<ExamTestPaper>(size, (index - 1) * size);
        }

        #endregion
    }
}
