using Song.Entities;
using Song.ServiceInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeiSha.Core;
using WeiSha.Data;

namespace Song.ServiceImpls
{
    public class ExamQuesCom : IExamQues
    {
        #region 试题
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public Questions QuesSingle(long id)
        {
            return Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
        }
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        public long QuesAdd(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls)
        {
            if (entity.Qus_ID <= 0) entity.Qus_ID = WeiSha.Core.Request.SnowID();
            entity.Qus_CrtTime = DateTime.Now;
            entity.Qus_LastTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            if (string.IsNullOrWhiteSpace(entity.Qus_UID)) entity.Qus_UID = WeiSha.Core.Request.UniqueID();

            Gateway.Default.Save<Questions>(entity);
            //保存关键字的关联
            this.TagConnectionQues(tags, entity);
            //保存分类的关联
            this.PartConnectionQues(parts, entity.Qus_ID);
            //保存知识点的关联
            this.KnlConnectionQues(knls, entity.Qus_ID);

            //更新试题分类、标签、知识点的题量统计
            this.PartQusTotalUpdate(entity);
            this.TagQusTotalUpdate(entity);
            this.KnlQusTotalUpdate(entity);
            return entity.Qus_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的试题</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        public void QuesSave(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls)
        {
            entity.Qus_LastTime = DateTime.Now;
            entity.Qus_IsError = false;
            //获取科目名称
            if (entity.Sbj_ID > 0 && string.IsNullOrWhiteSpace(entity.Sbj_Name))
            {
                Subject sbj = Business.Do<ISubject>().SubjectSingle(entity.Sbj_ID);
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            if (entity.Qus_Type == 4)
            {
                if (string.IsNullOrWhiteSpace(entity.Qus_Answer) || string.IsNullOrWhiteSpace(entity.Qus_Answer))
                {
                    entity.Qus_IsError = true;
                    entity.Qus_ErrorInfo = "答案不得为空";
                }
            }
            Questions former = this.QuesSingle(entity.Qus_ID);  //获取之前的试题信息
            //保存
            Gateway.Default.Save<Questions>(entity);

            //保存关键字的关联
            this.TagConnectionQues(tags, entity);
            //保存分类的关联
            this.PartConnectionQues(parts, entity.Qus_ID);
            //保存知识点的关联
            this.KnlConnectionQues(knls, entity.Qus_ID);

            //更新试题分类、标签、知识点的题量统计
            this.PartQusTotalUpdate(entity);
            this.TagQusTotalUpdate(entity);
            this.KnlQusTotalUpdate(entity, former);
        }
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        public int QuesDelete(Questions entity)
        {
            int count = Gateway.Default.Update<Questions>(Questions._.Qus_IsDeleted, true, Questions._.Qus_ID == entity.Qus_ID);
            this.PartQusTotalUpdate(entity);
            return count;
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int QuesDelete(long id)
        {
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
            return this.QuesDelete(qus);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int QuesRecycle(long id)
        {
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
            this.PartQusTotalUpdate(qus);
            return Gateway.Default.Update<Questions>(Questions._.Qus_IsDeleted, false, Questions._.Qus_ID == id);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int QuesRemove(long id)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //删除笔记
                    Gateway.Default.Delete<Student_Notes>(Student_Notes._.Qus_ID == id);
                    //删除收藏记录
                    Gateway.Default.Delete<Student_Collect>(Student_Collect._.Qus_ID == id);
                    Gateway.Default.Delete<QuesCollect>(QuesCollect._.Qus_ID == id);
                    //删除错题记录
                    Gateway.Default.Delete<Student_Ques>(Student_Ques._.Qus_ID == id);

                    //删除关联记录
                    Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qus_ID == id);
                    Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qus_ID == id);
                    Gateway.Default.Delete<Questions_QTags>(Questions_QTags._.Qus_ID == id);
                    //
                    //删除试题
                    int n = tran.Delete<Questions>(Questions._.Qus_ID == id);
                    //删除图片等资源
                    WeiSha.Core.Upload.Get["Ques"].DeleteDirectory(id.ToString());
                    tran.Commit();
                    return n;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search"></param>
        /// <param name="isdeleted"></param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Questions> QuesPager(int orgid, string search, bool? isdeleted, long[] qpid, long[] tagid, long[] knlid,
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (isdeleted != null) wc.And(Questions._.Qus_IsDeleted == isdeleted);    //删除状态
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //按题干进行检索
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Questions._.Qus_Title.Contains(search));
            //题型
            if (type != null && type.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int t in type) wctype |= Questions._.Qus_Type == t;
                wc.And(wctype);
            }
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            countSum = section.Where(wc).Count();
            return section.Where(wc).OrderBy(Questions._.Qus_ID.Desc).ToList<Questions>(size, (index - 1) * size);
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        /// <param name="ques">要更新的试题对象</param>
        /// <param name="former">原来的试题对象</param>
        public void QuesStatisticalUpdate(Questions ques, Questions former)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;
            wc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
        }
        /// <summary>
        /// 获取试题数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="qpid"></param>
        /// <param name="tagid"></param>
        /// <param name="knlid"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <returns>试题类型，数量</returns>
        public Dictionary<int, int> QuesTotal(int orgid, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diff, bool? isUse, bool? isError, bool? isWrong)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (isdeleted != null) wc.And(Questions._.Qus_IsDeleted == isdeleted);    //删除状态
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            //试题类型
            string[] types = Business.Do<IQuestions>().QuestionTypes();
            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int i = 0; i < types.Length; i++)
            {
                //section.Where(wc && Questions._.Qus_Type == (i + 1));
                int total = section.Where(wc && Questions._.Qus_Type == (i + 1)).SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
                dic.Add(i + 1, total);
            }
            return dic;
        }
        #endregion

        #region 试题分类
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int PartAdd(QuesPart entity)
        {
            entity.Qp_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            //如果没有排序号，则自动计算
            if (entity.Qp_Order < 1)
            {
                object obj = Gateway.Default.Max<QuesPart>(QuesPart._.Qp_Order, QuesPart._.Org_ID == entity.Org_ID && QuesPart._.Qp_PID == entity.Qp_PID);
                entity.Qp_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesPart>(entity);
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public QuesPart PartIsExist(int orgid, long pid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesPart._.Org_ID == orgid;
            if (pid >= 0) wc &= QuesPart._.Qp_PID == pid;
            return Gateway.Default.From<QuesPart>().Where(wc && QuesPart._.Qp_Name == name.Trim()).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuesPart PartIsExist(QuesPart entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesPart._.Org_ID == entity.Org_ID;
            if (entity.Qp_PID >= 0) wc &= QuesPart._.Qp_PID == entity.Qp_PID;
            if (entity.Qp_ID > 0) wc &= QuesPart._.Qp_ID != entity.Qp_ID;
            return Gateway.Default.From<QuesPart>().Where(wc && QuesPart._.Qp_Name == entity.Qp_Name.Trim()).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PartSave(QuesPart entity)
        {
            //专业的id与pid不能相等
            if (entity.Qp_PID == entity.Qp_ID) throw new Exception("QuesPart table PID Can not be equal to ID");
            QuesPart old = this.PartSingle(entity.Qp_ID);
            if (old.Qp_PID != entity.Qp_PID)
            {
                object obj = Gateway.Default.Max<QuesPart>(QuesPart._.Qp_Order, QuesPart._.Org_ID == entity.Org_ID && QuesPart._.Qp_PID == entity.Qp_PID);
                entity.Qp_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            entity.Qp_UpdateTime = DateTime.Now;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesPart>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int PartUpdate(long qpid, Field[] fields, object[] objs)
        {
            return Gateway.Default.Update<QuesPart>(fields, objs, QuesPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int PartUpdate(long qpid, Field field, object obj)
        {
            return Gateway.Default.Update<QuesPart>(field, obj, QuesPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PartDelete(long id)
        {
            List<long> list = this.PartTreeID(id, 0);   //获取当前试题分类下的所有子试题分类id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qpid in list)
                        tran.Update<QuesPart>(QuesPart._.Qp_IsDeleted, true, QuesPart._.Qp_ID == qpid);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PartRemove(long id)
        {
            List<long> list = this.PartTreeID(id, 0);   //获取当前试题分类下的所有子试题分类id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qpid in list)
                    {
                        tran.Delete<Questions_QPart>(Questions_QPart._.Qp_ID == qpid);
                        tran.Delete<QuesPart>(QuesPart._.Qp_ID == qpid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int PartRecycle(long id)
        {
            return Gateway.Default.Update<QuesPart>(QuesPart._.Qp_IsDeleted, false, QuesPart._.Qp_ID == id);
        }
        /// <summary>
        /// 清空试题分类下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qpid"></param>
        public void PartClear(long qpid)
        {
            Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesPart PartSingle(long id)
        {
            return Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == id).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 按主键获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesPart> PartSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesPart._.Qp_ID == id[i]);
            return Gateway.Default.From<QuesPart>().Where(wc).ToList<QuesPart>();
        }
        /// <summary>
        /// 当前试题分类下的所有子试题分类id，包括自身
        /// </summary>
        /// <param name="qpid">当前试题分类id</param>
        /// <param name="orgid">试题分类所属机构的ID,如果小于等于零，则取从数据库读取qpid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        public List<long> PartTreeID(long qpid, int orgid)
        {
            List<long> list = new List<long>();
            //如果未设置机构id，则取当前专业所属机构
            if (orgid <= 0)
            {
                QuesPart part = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToFirst<QuesPart>();
                if (part == null) return list;
                orgid = part.Org_ID;
            }
            //取同一个机构下的所有专业
            List<QuesPart> sbjs = Gateway.Default.From<QuesPart>().Where(QuesPart._.Org_ID == orgid).ToList<QuesPart>();
            list = _parttreeid(qpid, sbjs);
            return list;
        }
        /// <summary>
        /// 当前试题分类下的所有子试题分类id
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public List<long> PartTreeID(long[] qpid, int orgid)
        {
            List<long> list = new List<long>();
            foreach (long qid in qpid)
            {
                List<long> tm = this.PartTreeID(qid, orgid);
                foreach (long t in tm) if (!list.Contains(t)) list.Add(t);
            }
            return list;
        }
        private List<long> _parttreeid(long id, List<QuesPart> parts)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            List<long> childs = parts.Where(s => s.Qp_PID == id).Select(s => s.Qp_ID).ToList();
            parts.RemoveAll(s => s.Qp_PID == id);
            for (int i = 0; i < childs.Count; i++)
            {
                list.Add(childs[i]);
                List<long> tm = _parttreeid(childs[i], parts);
                list.AddRange(tm.Except(list));
            }
            return list;
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题分类的id</param>
        /// <returns></returns>
        public string PartName(long id)
        {
            QuesPart entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == id)
                    .Select(new Field[] { QuesPart._.Qp_ID, QuesPart._.Qp_PID, QuesPart._.Qp_Name }).ToFirst<QuesPart>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath))
                        xpath = entity.Qp_Name;
                    else
                        xpath = entity.Qp_Name + "," + xpath;
                    id = entity.Qp_PID;
                }
            } while (entity != null && id > 0);
            return xpath;
        }
        /// <summary>
        /// 当前试题分类，是否有子试题分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题分类Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        public bool PartIsChildren(int orgid, long id, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(QuesPart._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<QuesPart>(wc && QuesPart._.Qp_PID == id);
            return count > 0;
        }
        /// <summary>
        /// 获取试题分类
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesPart> PartCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesPart._.Qp_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesPart._.Qp_Name.Contains(sear));
            if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
            return Gateway.Default.From<QuesPart>().Where(wc).OrderBy(QuesPart._.Qp_Order.Asc && QuesPart._.Qp_ID.Asc).ToList<QuesPart>(count);
        }
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesPart> PartParents(long qpid, bool isself)
        {
            Stack st = new Stack();
            QuesPart s = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToFirst<QuesPart>();
            if (isself) st.Push(s);
            while (s.Qp_PID != 0)
            {
                s = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == s.Qp_PID).ToFirst<QuesPart>();
                if (s == null) break;
                st.Push(s);
            }
            List<QuesPart> list = new List<QuesPart>();
            foreach (object obj in st)
            {
                list.Add((QuesPart)obj);
            }
            return list;
        }
        /// <summary>
        /// 计算试题分类数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        public int PartOfCount(int orgid, long pid, bool? isUse, bool children)
        {
            if (pid < 0)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                return Gateway.Default.Count<QuesPart>(wc);
            }
            //不包括子级，仅当前专业的直接下级专业
            if (!children)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
                return Gateway.Default.Count<QuesPart>(wc);
            }
            else
            {
                //包括子级，当前专业下的所有专业数
                List<long> list = new List<long>();
                //取同一个机构下的所有章节
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                List<QuesPart> parts = Gateway.Default.From<QuesPart>().Where(wc).ToList<QuesPart>();
                list = _parttreeid(pid, parts);
                return list.Count;
            }
        }
        /// <summary>
        /// 当前试题分类下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> PartQuestions(int orgid, long qpid, int qtype, bool? isUse, bool children, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            List<long> listqpid = children ? new List<long>() { qpid } : this.PartTreeID(qpid, orgid);
            WhereClip wc2 = new WhereClip();
            foreach (long l in listqpid) wc2.Or(Questions_QPart._.Qp_ID == l);
            wc.And(wc2);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);

        }
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int PartQusTotal(int orgid, long qpid, int qtype, bool? isUse, bool children)
        {
            return PartQusTotal(orgid, new long[] { qpid }, qtype, isUse, children);
        }
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int PartQusTotal(int orgid, long[] qpid, int qtype, bool? isUse, bool children)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            //计算所有子级
            if (qpid != null)
            {
                List<long> listqpid = new List<long>();
                if (!children) listqpid = qpid.ToList();
                else
                {
                    foreach (long id in qpid)
                    {
                        List<long> list = this.PartTreeID(id, orgid);
                        foreach (long l in list)
                        {
                            if (!listqpid.Contains(l)) listqpid.Add(l);
                        }
                    }
                }
                WhereClip wc2 = new WhereClip();
                foreach (long l in listqpid) wc2.Or(Questions_QPart._.Qp_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void PartQusTotalUpdate(List<QuesPart> parts)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的分类
            foreach (QuesPart part in parts)
            {
                //分类的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QPart._.Qp_ID == part.Qp_ID).Count();
                Gateway.Default.Update<QuesPart>(QuesPart._.QP_Count, partcount, QuesPart._.Qp_ID == part.Qp_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void PartQusTotalUpdate(Questions ques)
        {
            List<QuesPart> list = this.PartForQues(ques.Qus_ID);
            if (list == null || list.Count <= 0) return;
            this.PartQusTotalUpdate(list);
        }
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        public void PartQusTotalUpdate()
        {
            List<QuesPart> list = Gateway.Default.From<QuesPart>().ToList<QuesPart>();
            if (list == null || list.Count <= 0) return;
            this.PartQusTotalUpdate(list);
        }
        /// <summary>
        /// 试题所属的分类，由于是多对多关联，试题可能会属于多个分类
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesPart> PartForQues(long quesid)
        {
            return Gateway.Default.From<QuesPart>().LeftJoin<Questions_QPart>(Questions_QPart._.Qp_ID == QuesPart._.Qp_ID).Where(Questions_QPart._.Qus_ID == quesid).ToList<QuesPart>();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesPart> PartPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
            if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesPart._.Qp_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesPart._.Qp_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesPart>(wc);
            return Gateway.Default.From<QuesPart>().Where(wc).OrderBy(QuesPart._.Qp_Order.Asc).ToList<QuesPart>(size, (index - 1) * size);
        }
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">试题分类列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        public bool PartUpdateTaxis(QuesPart[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.QuesPart part in list)
                    {
                        tran.Update<QuesPart>(
                            new Field[] { QuesPart._.Qp_PID, QuesPart._.Qp_Order },
                            new object[] { part.Qp_PID, part.Qp_Order },
                            QuesPart._.Qp_ID == part.Qp_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="qusid"></param> 
        /// <returns></returns>
        public int PartConnectionQues(long qpid, long qusid)
        {
            if (qpid <= 0 || qusid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qusid).ToFirst<Questions>();
            QuesPart[] parts = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToArray<QuesPart>();          
            return PartConnectionQues(parts, ques);
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        public int PartConnectionQues(QuesPart[] parts, long qusid)
        {
            if (qusid <= 0) return 0;
            Questions ques = this.QuesSingle(qusid);
            return PartConnectionQues(parts, ques);
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        public int PartConnectionQues(QuesPart[] parts, Questions ques)
        {
            //原有试题分类
            List<QuesPart> list = this.PartForQues(ques.Qus_ID);

            //删除原有与试题分类的关联
            Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesPart part in parts)
            {               
                WhereClip wc = Questions_QPart._.Qus_ID == ques.Qus_ID && Questions_QPart._.Qp_ID == part.Qp_ID;
                Questions_QPart qqpart = Gateway.Default.From<Questions_QPart>().Where(wc).ToFirst<Questions_QPart>();
                if (qqpart != null) return 0;
                qqpart = new Questions_QPart()
                {
                    Qus_ID = ques.Qus_ID,
                    Qp_ID = part.Qp_ID
                };
                i += Gateway.Default.Save<Questions_QPart>(qqpart);
                if (!list.Any(p => p.Qp_ID == part.Qp_ID)) list.Add(part);
            }
            //更新分类的试题数统计
            this.PartQusTotalUpdate(list);
            return i;
        }
        #endregion

        #region 收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public bool CollectAdd(int accid, long qusid)
        {
            QuesCollect qc = Gateway.Default.From<QuesCollect>().Where(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid).ToFirst<QuesCollect>();
            if (qc != null) return false;
            return Gateway.Default.Insert<QuesCollect>(new QuesCollect()
            {
                Acc_ID = accid,
                Qus_ID = qusid,
                Qcl_CrtTime = DateTime.Now
            }) > 0;
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public bool CollectRemove(int accid, long qusid)
        {
            Gateway.Default.Delete<QuesCollect>(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid);
            return true;
        }
        /// <summary>
        /// 批量取消收藏
        /// </summary>
        public int CollectRemove(int accid, long[] qusid)
        {
            int i = 0;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long l in qusid)
                    {
                        i += tran.Delete<QuesCollect>(QuesCollect._.Qus_ID == l && QuesCollect._.Acc_ID == accid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        public bool Collected(int accid, long qusid)
        {
            if (accid <= 0 || qusid <= 0) return false;
            return Gateway.Default.Count<QuesCollect>(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid) > 0;
        }
        /// <summary>
        /// 获取收藏的试题
        /// </summary>
        /// <param name="acid">管理员id</param>
        /// <param name="search">搜索题干</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">试题标签id</param>
        /// <param name="knlid">试题知识点id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否有格式错误</param>
        /// <param name="isWrong">是否有反馈的错误</param>
        /// <param name="size">分页大小</param>
        /// <param name="index">分页索引</param>
        /// <param name="countSum">总记录数</param>
        /// <returns></returns>
        public List<Questions> CollectPager(int acid, string search, long[] qpid, long[] tagid, long[] knlid,
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            wc.And(QuesCollect._.Acc_ID == acid);
            wc.And(Questions._.Qus_IsDeleted == false);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);

            //按题干进行检索
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Questions._.Qus_Title.Contains(search));
            //题型
            if (type != null && type.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int t in type) wctype |= Questions._.Qus_Type == t;
                wc.And(wctype);
            }
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<QuesCollect>(QuesCollect._.Qus_ID == Questions._.Qus_ID);
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long d in qpid) wcqp |= Questions_QPart._.Qp_ID == d;
                wc.And(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wc.And(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long k in knlid) wcqp |= Questions_QKnl._.Qk_ID == k;
                wc.And(wcqp);
            }
            countSum = section.Where(wc).Count();
            return section.Where(wc).OrderBy(Questions._.Qus_ID.Desc).ToList<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 试题知识点
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int KnlAdd(QuesKnowledge entity)
        {
            entity.Qk_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            //如果没有排序号，则自动计算
            if (entity.Qk_Order < 1)
            {
                object obj = Gateway.Default.Max<QuesKnowledge>(QuesKnowledge._.Qk_Order, QuesKnowledge._.Org_ID == entity.Org_ID && QuesKnowledge._.Qk_PID == entity.Qk_PID);
                entity.Qk_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesKnowledge>(entity);
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public QuesKnowledge KnlIsExist(int orgid, long pid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesKnowledge._.Org_ID == orgid;
            if (pid >= 0) wc &= QuesKnowledge._.Qk_PID == pid;
            return Gateway.Default.From<QuesKnowledge>().Where(wc && QuesKnowledge._.Qk_Name == name.Trim()).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuesKnowledge KnlIsExist(QuesKnowledge entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesKnowledge._.Org_ID == entity.Org_ID;
            if (entity.Qk_PID >= 0) wc &= QuesKnowledge._.Qk_PID == entity.Qk_PID;
            if (entity.Qk_ID > 0) wc &= QuesKnowledge._.Qk_ID != entity.Qk_ID;
            return Gateway.Default.From<QuesKnowledge>().Where(wc && QuesKnowledge._.Qk_Name == entity.Qk_Name.Trim()).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void KnlSave(QuesKnowledge entity)
        {
            //专业的id与pid不能相等
            if (entity.Qk_PID == entity.Qk_ID) throw new Exception("QuesKnowledge table PID Can not be equal to ID");
            QuesKnowledge old = this.KnlSingle(entity.Qk_ID);
            if (old.Qk_PID != entity.Qk_PID)
            {
                object obj = Gateway.Default.Max<QuesKnowledge>(QuesKnowledge._.Qk_Order, QuesKnowledge._.Org_ID == entity.Org_ID && QuesKnowledge._.Qk_PID == entity.Qk_PID);
                entity.Qk_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            entity.Qk_UpdateTime = DateTime.Now;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesKnowledge>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int KnlUpdate(long qkid, Field[] fields, object[] objs)
        {
            return Gateway.Default.Update<QuesKnowledge>(fields, objs, QuesKnowledge._.Qk_ID == qkid);
        }
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int KnlUpdate(long qkid, Field field, object obj)
        {
            return Gateway.Default.Update<QuesKnowledge>(field, obj, QuesKnowledge._.Qk_ID == qkid);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int KnlDelete(long id)
        {
            List<long> list = this.KnlTreeID(id, 0);   //获取当前试题知识点下的所有子试题知识点id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qkid in list)
                        tran.Update<QuesKnowledge>(QuesKnowledge._.Qk_IsDeleted, true, QuesKnowledge._.Qk_ID == qkid);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int KnlRemove(long id)
        {
            List<long> list = this.KnlTreeID(id, 0);   //获取当前试题知识点下的所有子试题知识点id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qkid in list)
                    {
                        tran.Delete<Questions_QKnl>(Questions_QKnl._.Qk_ID == qkid);
                        tran.Delete<QuesKnowledge>(QuesKnowledge._.Qk_ID == qkid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int KnlRecycle(long id)
        {
            return Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_IsDeleted, false, QuesKnowledge._.Qk_ID == id);
        }
        /// <summary>
        /// 清空试题知识点下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qkid"></param>
        public void KnlClear(long qkid)
        {
            Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qk_ID == qkid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesKnowledge KnlSingle(long id)
        {
            return Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == id).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 按ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesKnowledge._.Qk_ID == id[i]);
            return Gateway.Default.From<QuesKnowledge>().Where(wc).ToList<QuesKnowledge>();
        }
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id，包括自身
        /// </summary>
        /// <param name="qkid">当前试题知识点id</param>
        /// <param name="orgid">试题知识点所属机构的ID,如果小于等于零，则取从数据库读取qkid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        public List<long> KnlTreeID(long qkid, int orgid)
        {
            List<long> list = new List<long>();
            //如果未设置机构id，则取当前专业所属机构
            if (orgid <= 0)
            {
                QuesKnowledge Knl = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToFirst<QuesKnowledge>();
                if (Knl == null) return list;
                orgid = Knl.Org_ID;
            }
            //取同一个机构下的所有专业
            List<QuesKnowledge> sbjs = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Org_ID == orgid).ToList<QuesKnowledge>();
            list = _knltreeid(qkid, sbjs);
            return list;
        }
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id
        /// </summary>
        public List<long> KnlTreeID(long[] qkid, int orgid)
        {
            List<long> list = new List<long>();
            foreach (long kid in qkid)
            {
                List<long> tm = this.KnlTreeID(kid, orgid);
                foreach (long t in tm) if (!list.Contains(t)) list.Add(t);
            }
            return list;
        }
        private List<long> _knltreeid(long id, List<QuesKnowledge> Knls)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            List<long> childs = Knls.Where(s => s.Qk_PID == id).Select(s => s.Qk_ID).ToList();
            Knls.RemoveAll(s => s.Qk_PID == id);
            for (int i = 0; i < childs.Count; i++)
            {
                list.Add(childs[i]);
                List<long> tm = _knltreeid(childs[i], Knls);
                list.AddRange(tm.Except(list));
            }
            return list;
        }
        /// <summary>
        /// 获取试题知识点名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题知识点的id</param>
        /// <returns></returns>
        public string KnlName(long id)
        {
            QuesKnowledge entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == id)
                    .Select(new Field[] { QuesKnowledge._.Qk_ID, QuesKnowledge._.Qk_PID, QuesKnowledge._.Qk_Name }).ToFirst<QuesKnowledge>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath))
                        xpath = entity.Qk_Name;
                    else
                        xpath = entity.Qk_Name + "," + xpath;
                    id = entity.Qk_PID;
                }
            } while (entity != null && id > 0);
            return xpath;
        }
        /// <summary>
        /// 当前试题知识点，是否有子试题知识点
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题知识点Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        public bool KnlIsChildren(int orgid, long id, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<QuesKnowledge>(wc && QuesKnowledge._.Qk_PID == id);
            return count > 0;
        }
        /// <summary>
        /// 获取试题知识点
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesKnowledge._.Qk_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesKnowledge._.Qk_Name.Contains(sear));
            if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
            return Gateway.Default.From<QuesKnowledge>().Where(wc).OrderBy(QuesKnowledge._.Qk_Order.Asc && QuesKnowledge._.Qk_ID.Asc).ToList<QuesKnowledge>(count);
        }
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlParents(long qkid, bool isself)
        {
            Stack st = new Stack();
            QuesKnowledge s = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToFirst<QuesKnowledge>();
            if (isself) st.Push(s);
            while (s.Qk_PID != 0)
            {
                s = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == s.Qk_PID).ToFirst<QuesKnowledge>();
                if (s == null) break;
                st.Push(s);
            }
            List<QuesKnowledge> list = new List<QuesKnowledge>();
            foreach (object obj in st)
            {
                list.Add((QuesKnowledge)obj);
            }
            return list;
        }
        /// <summary>
        /// 计算试题知识点数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        public int KnlOfCount(int orgid, long pid, bool? isUse, bool children)
        {
            if (pid < 0)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                return Gateway.Default.Count<QuesKnowledge>(wc);
            }
            //不包括子级，仅当前专业的直接下级专业
            if (!children)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
                return Gateway.Default.Count<QuesKnowledge>(wc);
            }
            else
            {
                //包括子级，当前专业下的所有专业数
                List<long> list = new List<long>();
                //取同一个机构下的所有章节
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                List<QuesKnowledge> Knls = Gateway.Default.From<QuesKnowledge>().Where(wc).ToList<QuesKnowledge>();
                list = _knltreeid(pid, Knls);
                return list.Count;
            }
        }
        /// <summary>
        /// 当前试题知识点下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> KnlQuestions(int orgid, long qkid, int qtype, bool? isUse, bool children, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            List<long> listqkid = children ? new List<long>() { qkid } : this.KnlTreeID(qkid, orgid);
            WhereClip wc2 = new WhereClip();
            foreach (long l in listqkid) wc2.Or(Questions_QKnl._.Qk_ID == l);
            wc.And(wc2);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);

        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int KnlQusTotal(int orgid, long qkid, int qtype, bool? isUse, bool children)
        {
            return KnlQusTotal(orgid, new long[] { qkid }, qtype, isUse, children);
        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int KnlQusTotal(int orgid, long[] qkid, int qtype, bool? isUse, bool children)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            //计算所有子级
            if (qkid != null)
            {
                List<long> listqkid = new List<long>();
                if (!children) listqkid = qkid.ToList();
                else
                {
                    foreach (long id in qkid)
                    {
                        List<long> list = this.KnlTreeID(id, orgid);
                        foreach (long l in list)
                        {
                            if (!listqkid.Contains(l)) listqkid.Add(l);
                        }
                    }
                }
                WhereClip wc2 = new WhereClip();
                foreach (long l in listqkid) wc2.Or(Questions_QKnl._.Qk_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(wc);  
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        /// <param name="ques">要更新的试题对象</param>
        /// <param name="former">原来的试题对象</param>
        public void KnlQusTotalUpdate(Questions ques, Questions former)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的分类
            QuerySection<QuesKnowledge> section = null;
            if (ques == null || former == null) section = Gateway.Default.From<QuesKnowledge>().Where(new WhereClip());
            else
            {
                WhereClip wc = new WhereClip();
                if (ques != null) wc |= Questions_QKnl._.Qus_ID == ques.Qus_ID;
                if (former != null) wc |= Questions_QKnl._.Qus_ID == former.Qus_ID;
                section = Gateway.Default.From<QuesKnowledge>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qk_ID == QuesKnowledge._.Qk_ID).Where(wc);
            }
            //当前试题关联的知识点      
            using (SourceReader reader = section.ToReader())
            {
                while (reader.Read())
                {
                    object dvalue = reader["Qk_ID"];
                    long qkid = Convert.ToInt64(dvalue);
                    //知识点的试题数
                    int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QKnl._.Qk_ID == qkid).Count();
                    Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_Count, partcount, QuesKnowledge._.Qk_ID == qkid);
                }
                reader.Close();
                reader.Dispose();
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary> 
        public void KnlQusTotalUpdate(List<QuesKnowledge> knls)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的知识点
            foreach (QuesKnowledge knl in knls)
            {
                //知识点的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QKnl._.Qk_ID == knl.Qk_ID).Count();
                Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_Count, partcount, QuesKnowledge._.Qk_ID == knl.Qk_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void KnlQusTotalUpdate(Questions ques)
        {
            List<QuesKnowledge> list = this.KnlForQues(ques.Qus_ID);
            if (list == null || list.Count <= 0) return;
            this.KnlQusTotalUpdate(list);
        }
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        public void KnlQusTotalUpdate()
        {
            List<QuesKnowledge> list = Gateway.Default.From<QuesKnowledge>().ToList<QuesKnowledge>();
            if (list == null || list.Count <= 0) return;
            this.KnlQusTotalUpdate(list);
        }
        /// <summary>
        /// 试题关联的知识点
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlForQues(long quesid)
        {
            return Gateway.Default.From<QuesKnowledge>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qk_ID == QuesKnowledge._.Qk_ID).Where(Questions_QKnl._.Qus_ID == quesid).ToList<QuesKnowledge>();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesKnowledge._.Qk_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesKnowledge._.Qk_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesKnowledge>(wc);
            return Gateway.Default.From<QuesKnowledge>().Where(wc).OrderBy(QuesKnowledge._.Qk_Order.Asc).ToList<QuesKnowledge>(size, (index - 1) * size);
        }
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">试题知识点列表，对象中只有Qk_ID、Qk_PID、Qk_Order</param>
        /// <returns></returns>
        public bool KnlUpdateTaxis(QuesKnowledge[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.QuesKnowledge Knl in list)
                    {
                        tran.Update<QuesKnowledge>(
                            new Field[] { QuesKnowledge._.Qk_PID, QuesKnowledge._.Qk_Order },
                            new object[] { Knl.Qk_PID, Knl.Qk_Order },
                            QuesKnowledge._.Qk_ID == Knl.Qk_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        /// <param name="qkid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        public int KnlConnectionQues(long qkid, long qusid)
        {
            if (qusid <= 0 || qusid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qusid).ToFirst<Questions>();
            QuesKnowledge[] knls = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToArray<QuesKnowledge>();
            return KnlConnectionQues(knls, ques);
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        public int KnlConnectionQues(QuesKnowledge[] knls, long qusid)
        {
            if (qusid <= 0) return 0;
            Questions ques = this.QuesSingle(qusid);
            return this.KnlConnectionQues(knls, ques);           
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        public int KnlConnectionQues(QuesKnowledge[] knls, Questions ques)
        {
            //原有试题知识点
            List<QuesKnowledge> list = this.KnlForQues(ques.Qus_ID);

            //删除原有与试题知识点的关联
            Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesKnowledge knl in knls)
            {
                Questions_QKnl qqknl = new Questions_QKnl()
                {
                    Qus_ID = ques.Qus_ID,
                    Qk_ID = knl.Qk_ID
                };
                i += Gateway.Default.Save<Questions_QKnl>(qqknl);
                if (!list.Any(p => p.Qk_ID == knl.Qk_ID)) list.Add(knl);
            }
            //更新知识点的试题数统计
            this.KnlQusTotalUpdate(list);
            return i;
        }
        #endregion

        #region 关键字
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int TagAdd(QuesTags entity)
        {
            entity.Qtag_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            //如果没有排序号，则自动计算
            if (entity.Qtag_Order < 1)
            {
                WhereClip wc = QuesTags._.Org_ID == entity.Org_ID;
                if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
                object obj = Gateway.Default.Max<QuesTags>(QuesTags._.Qtag_Order, wc);
                entity.Qtag_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesTags>(entity);
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TagIsExist(int orgid, long couid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesTags._.Org_ID == orgid;
            if (couid >= 0) wc &= QuesTags._.Cou_ID == couid;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool TagIsExist(QuesTags entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesTags._.Org_ID == entity.Org_ID;
            if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
            if (entity.Qtag_ID > 0) wc &= QuesTags._.Qtag_ID != entity.Qtag_ID;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == entity.Qtag_Name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="oneself">是否包括自身</param>
        /// <returns></returns>
        public bool TagIsExist(QuesTags entity, bool oneself = false)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesTags._.Org_ID == entity.Org_ID;
            if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
            if (!oneself && entity.Qtag_ID > 0) wc &= QuesTags._.Qtag_ID != entity.Qtag_ID;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == entity.Qtag_Name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TagSave(QuesTags entity)
        {
            entity.Qtag_UpdateTime = DateTime.Now;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesTags>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int TagDelete(long id)
        {
            return Gateway.Default.Update<QuesTags>(QuesTags._.Qtag_IsDeleted, true, QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int TagRecycle(long id)
        {
            return Gateway.Default.Update<QuesTags>(QuesTags._.Qtag_IsDeleted, false, QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int TagRemove(long id)
        {
            return Gateway.Default.Delete<QuesTags>(QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesTags TagSingle(long id)
        {
            return Gateway.Default.From<QuesTags>().Where(QuesTags._.Qtag_ID == id).ToFirst<QuesTags>();
        }
        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesTags> TagSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesTags._.Qtag_ID == id[i]);
            return Gateway.Default.From<QuesTags>().Where(wc).ToList<QuesTags>();
        }
        /// <summary>
        /// 获取单一实体对象，按主键名称；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public QuesTags TagSingle(string name, int orgid, long couid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            wc.And(QuesTags._.Qtag_Name == name);
            return Gateway.Default.From<QuesTags>().Where(wc).ToFirst<QuesTags>();
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public int TagConnectionQues(string tag, long quesid, long couid)
        {
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            if (quesid <= 0) return 0;
            QuesTags qtag = this.TagSingle(tag, ques.Org_ID, couid > 0 ? couid : ques.Cou_ID);
            if (qtag == null)
            {
                qtag = new QuesTags()
                {
                    Qtag_Name = tag,
                    Qtag_Weight = 0,
                    Org_ID = ques.Org_ID,
                };
                this.TagAdd(qtag);
            }
            return this.TagConnectionQues(new QuesTags[] { qtag }, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public int TagConnectionQues(long tagid, long quesid, long couid)
        {
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            QuesTags[] tags = Gateway.Default.From<QuesTags>().Where(QuesTags._.Qtag_ID == tagid).ToArray<QuesTags>();
            ques.Cou_ID = couid;
            return TagConnectionQues(tags, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        public int TagConnectionQues(QuesTags[] tags, long quesid, long couid)
        {
            if (quesid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            ques.Cou_ID = couid;
            return TagConnectionQues(tags, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        public int TagConnectionQues(QuesTags[] tags, Questions ques)
        {
            //原有试题标签
            List<QuesTags> list = this.TagForQues(ques.Qus_ID);

            //删除原有与试题分类的关联
            Gateway.Default.Delete<Questions_QTags>(Questions_QTags._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesTags tag in tags)
            {
                if (!this.TagIsExist(tag, true)) this.TagAdd(tag);
                WhereClip wc = Questions_QTags._.Qus_ID == ques.Qus_ID && Questions_QTags._.Qtag_ID == tag.Qtag_ID;
                Questions_QTags qqtag = Gateway.Default.From<Questions_QTags>().Where(wc).ToFirst<Questions_QTags>();
                if (qqtag == null)
                {
                    qqtag = new Questions_QTags()
                    {
                        Qus_ID = ques.Qus_ID,
                        Qtag_ID = tag.Qtag_ID
                    };
                }
                i += Gateway.Default.Save<Questions_QTags>(qqtag);
                if (!list.Any(p => p.Qtag_ID == tag.Qtag_ID)) list.Add(tag);
            }
            this.TagQusTotalUpdate(list);
            return i;
        }
        /// <summary>
        /// 获取试题标签
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesTags> TagCount(int orgid, string sear, long couid, bool? isdeleted, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesTags._.Qtag_Name.Contains(sear));
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            return Gateway.Default.From<QuesTags>().Where(wc).OrderBy(QuesTags._.Qtag_Order.Asc).ToList<QuesTags>(count);
        }
        /// <summary>
        /// 计算试题标签的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param> 
        /// <returns></returns>
        public int TagOfCount(int orgid, long couid, bool? isdeleted)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            return Gateway.Default.Count<QuesTags>(wc);
        }
        /// <summary>
        /// 当前试题标签下的所有试题
        /// </summary>
        /// <param name="qtagid"></param>
        /// <param name="couid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> TagQuestions(long qtagid, long couid, int qtype, bool? isuse, int count)
        {

            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            wc &= Questions._.Qus_IsDeleted == false;

            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);
        }
        /// <summary>
        /// 某道试题的关键字
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesTags> TagForQues(long quesid)
        {
            return Gateway.Default.From<QuesTags>().LeftJoin<Questions_QTags>(Questions_QTags._.Qtag_ID == QuesTags._.Qtag_ID)
                .Where(Questions_QTags._.Qus_ID == quesid).ToList<QuesTags>();
        }
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <returns></returns>
        public int TagQusTotal(long qtagid, long couid, int qtype, bool? isuse)
        {
            return TagQusTotal(new long[] { qtagid }, couid, qtype, isuse);
        }
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="isuse"></param>
        public int TagQusTotal(long[] qtagid, long couid, int qtype, bool? isuse)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            wc &= Questions._.Qus_IsDeleted == false;

            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            if (qtagid != null)
            {
                WhereClip wc2 = new WhereClip();
                foreach (long l in qtagid) wc2.Or(Questions_QTags._.Qtag_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        public void TagQusTotalUpdate(List<QuesTags> tags)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //试题的标签
            foreach (QuesTags tag in tags)
            {
                //标签的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QTags._.Qtag_ID == tag.Qtag_ID).Count();
                Gateway.Default.Update<QuesTags>(QuesTags._.Qtag_Count, partcount, QuesTags._.Qtag_ID == tag.Qtag_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        public void TagQusTotalUpdate(Questions ques)
        {
            if (ques == null) return;
            List<QuesTags> tags = this.TagForQues(ques.Qus_ID);
            if (tags == null || tags.Count <= 0) return;
            this.TagQusTotalUpdate(tags);
        }
        /// <summary>
        /// 更新试题标签所有试题数量
        /// </summary>
        public void TagQusTotalUpdate()
        {
            List<QuesTags> list = Gateway.Default.From<QuesTags>().ToList<QuesTags>();
            if (list == null || list.Count <= 0) return;
            this.TagQusTotalUpdate(list);
        }
        /// <summary>
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesTags> TagPager(int orgid, long couid, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesTags._.Qtag_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesTags>(wc);
            return Gateway.Default.From<QuesTags>().Where(wc).OrderBy(QuesTags._.Qtag_Order.Asc).ToList<QuesTags>(size, (index - 1) * size);
        }
        #endregion
    }
}
