using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace Song.ServiceImpls
{
    public class ExamQuesCom : IExamQues
    {
        #region 试题
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        public int QuesDelete(Questions entity)
        {
            return Gateway.Default.Update<Questions>(Questions._.Qus_IsDeleted, true, Questions._.Qus_ID == entity.Qus_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int QuesDelete(long id)
        {
            return Gateway.Default.Update<Questions>(Questions._.Qus_IsDeleted, true, Questions._.Qus_ID == id);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int QuesRecycle(long id)
        {
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
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Questions> QuesPager(int orgid, long[] qpid, long[] tagid, long[] knlid, int[] type, int[] diff, int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);


            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Desc).ToList<Questions>(size, (index - 1) * size);
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
                        tran.Update<QuesPart>(QuesPart._.Qp_IsDeleted,true, QuesPart._.Qp_ID == qpid);                  
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
            if (orgid >= 0) wc.And(QuesPart._.Org_ID == orgid);
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
            return section.Count();
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
                            new object[] { part.Qp_PID, part.Qp_Order},
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
            QuesCollect qc=Gateway.Default.From<QuesCollect>().Where(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid).ToFirst<QuesCollect>();
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
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">试题标签id</param>
        /// <param name="knlid">试题知识点id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="size">分页大小</param>
        /// <param name="index">分页索引</param>
        /// <param name="countSum">总记录数</param>
        /// <returns></returns>
        public List<Questions> CollectPager(int acid, long[] qpid, long[] tagid, long[] knlid, int[] type, int[] diff, int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            wc.And(QuesCollect._.Acc_ID == acid);
            //countSum = Gateway.Default.Count<Questions>(wc);
            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<QuesCollect>(QuesCollect._.Qus_ID == Questions._.Qus_ID).Where(wc);
            countSum = section.Count();

            return section.OrderBy(Questions._.Qus_ID.Desc).ToList<Questions>(size, (index - 1) * size);
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
            if (orgid >= 0) wc.And(QuesKnowledge._.Org_ID == orgid);
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
            return section.Count();
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
        #endregion

        #region 关键字
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int TagAdd(QuesTags entity)
        {
            if (this.TagIsExist(entity)) return 0;
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
            if (entity.Cou_ID >= 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
            if (entity.Qtag_ID > 0) wc &= QuesTags._.Qtag_ID != entity.Qtag_ID;
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
            if(isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);
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
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            wc &= Questions._.Qus_IsDeleted == false;

            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.Count();
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
