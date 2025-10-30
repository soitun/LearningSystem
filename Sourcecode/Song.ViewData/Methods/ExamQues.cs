using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Song.ViewData;
using Help = Song.ViewData.Helper;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试专用题库的管理，有区别于课程中的题库
    /// </summary>
    public class ExamQues : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 逻辑删除试题
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int QuesDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().QuesDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int QuesRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().QuesRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题分
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int QuesRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().QuesRemove(s);
            return i;
        }
        /// <summary>
        /// 获取题库列表
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">试题分类</param>
        /// <param name="tagid">标签</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难度</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult QuesPager(int orgid, string qpid, string tagid, string knlid, string type, string diff, int size, int index)
        {
            int sum;
            List<Questions> list = Business.Do<IExamQues>().QuesPager(orgid,
                Help.StringTo.Array<long>(qpid),
                Help.StringTo.Array<long>(tagid),
                Help.StringTo.Array<long>(knlid),
                Help.StringTo.Array<int>(type),
                Help.StringTo.Array<int>(diff),
                size, index, out sum);

            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }

        #region 试题收藏
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="qusid">试题id</param>
        /// <param name="accid">管理id</param>
        /// <returns></returns>
        public bool Collected(int accid, long qusid)
        {
            return Business.Do<IExamQues>().Collected(accid, qusid);
        }
        /// <summary>
        /// 收藏试题
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CollectAdd(int accid, long qusid) => Business.Do<IExamQues>().CollectAdd(accid, qusid);
        /// <summary>
        /// 取消试题收藏
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        [Admin] 
        [HttpDelete, HttpGet(Ignore = true)]
        public bool CollectRemove(int accid, long qusid) => Business.Do<IExamQues>().CollectRemove(accid, qusid);

        /// <summary>
        /// 收藏状态的变更
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <param name="state">收藏状态，为true时创建收藏关联，否则删除收藏</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CollectUpdate(int accid, long qusid,bool state)
        {
            if(state) return Business.Do<IExamQues>().CollectAdd(accid, qusid);
            else return Business.Do<IExamQues>().CollectRemove(accid, qusid);
        }
        /// <summary>
        /// 管理员收藏的试题
        /// </summary>
        /// <param name="acid">管理员的id</param>
        /// <param name="qpid">试题分类的id，多个id用逗号分隔</param>
        /// <param name="tagid"></param>
        /// <param name="knlid"></param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult CollectPager(int acid, string qpid, string tagid, string knlid, string type, string diff, int size, int index)
        {
            int sum;
            List<Questions> list = Business.Do<IExamQues>().CollectPager(acid,
                Help.StringTo.Array<long>(qpid),
                Help.StringTo.Array<long>(tagid),
                Help.StringTo.Array<long>(knlid),
                Help.StringTo.Array<int>(type),
                Help.StringTo.Array<int>(diff),
                size, index, out sum);

            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }

        #endregion

        #region 试题分类

        /// <summary>
        /// 获取试题分类的单条数据
        /// </summary>
        /// <param name="id">试题分类id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesPart PartForID(long id)
        {
            return Business.Do<IExamQues>().PartSingle(id);  
        }
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid">当前试题分类的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesPart> PartParents(long qpid, bool isself) => Business.Do<IExamQues>().PartParents(qpid, isself);
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">试题分类的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesPart PartAdd(Song.Entities.QuesPart entity)
        {
            Business.Do<IExamQues>().PartAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改试题分类
        /// </summary>
        /// <param name="entity">试题分类的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesPart PartModify(Song.Entities.QuesPart entity)
        {
            Song.Entities.QuesPart old = Business.Do<IExamQues>().PartSingle(entity.Qp_ID);
            if (old == null) throw new Exception("Not found entity for QuesPart！");          
            try
            { 
                old.Copy<Song.Entities.QuesPart>(entity);
                Business.Do<IExamQues>().PartSave(old);
                return entity;
            }
            catch (Exception ex)
            {               
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题分类，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题分类id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int PartDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题分类
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int PartRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题分类，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题分类id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int PartRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">上级id</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult PartPager(int orgid, long pid, bool? isuse, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesPart> arr = Business.Do<IExamQues>().PartPager(orgid, pid, isuse, isdeleted, name, size, index, out count);            
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个机构下的专业，用于前端展示，被禁用的专业不显示
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        [Cache]
        public JArray PartTreeFront(int orgid)
        {
            List<Song.Entities.QuesPart> sbjs = Business.Do<IExamQues>().PartCount(orgid, string.Empty, true, false, -1, -1);
            return sbjs.Count > 0 ? _partsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 机构下的专业，树形数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public JArray PartTree(int orgid, string search, bool? isuse)
        {
            List<Song.Entities.QuesPart> sbjs = Business.Do<IExamQues>().PartCount(orgid, search, isuse, false, -1, -1);
            return sbjs.Count > 0 ? _partsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _partsNode(Song.Entities.QuesPart item, List<Song.Entities.QuesPart> items)
        {
            List<Song.Entities.QuesPart> childs = new List<Song.Entities.QuesPart>();
            for (int i = 0; i < items.Count; i++)
            {
                Entities.QuesPart m = items[i];
                if (item == null && m.Qp_PID != 0) continue;
                if (item != null && m.Qp_PID != item.Qp_ID) continue;
                childs.Add(m);
                items.RemoveAt(i);
                i--;
            }
            JArray jarr = new JArray();
            for (int i = 0; i < childs.Count; i++)
            {
                string j = childs[i].ToJson("", "Qp_CrtTime,Qp_UpdateTime");
                JObject jo = JObject.Parse(j);
                jarr.Add(jo);
                //计算下级
                JArray charray = _partsNode(childs[i], items);
                if (charray.Count > 0) jo.Add("children", charray);
            }
            return jarr;
        }
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">专业的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool PartModifyTaxis(Song.Entities.QuesPart[] list)
        {
            try
            {
                Business.Do<IExamQues>().PartUpdateTaxis(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            => Business.Do<IExamQues>().PartQusTotal(orgid, qpid, qtype, isUse, children);
        #endregion

        #region 试题知识点

        /// <summary>
        /// 获取试题知识点的单条数据
        /// </summary>
        /// <param name="id">试题知识点id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesKnowledge KnlForID(long id)
        {
            return Business.Do<IExamQues>().KnlSingle(id);
        }
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid">当前试题知识点的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlParents(long qkid, bool isself) => Business.Do<IExamQues>().KnlParents(qkid, isself);
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">试题知识点的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesKnowledge KnlAdd(Song.Entities.QuesKnowledge entity)
        {
            Business.Do<IExamQues>().KnlAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改试题知识点
        /// </summary>
        /// <param name="entity">试题知识点的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesKnowledge KnlModify(Song.Entities.QuesKnowledge entity)
        {
            Song.Entities.QuesKnowledge old = Business.Do<IExamQues>().KnlSingle(entity.Qk_ID);
            if (old == null) throw new Exception("Not found entity for QuesKnowledge！");
            try
            {
                old.Copy<Song.Entities.QuesKnowledge>(entity);
                Business.Do<IExamQues>().KnlSave(old);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int KnlDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题知识点
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int KnlRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int KnlRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题知识点
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">上级id</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult KnlPager(int orgid, long pid, bool? isuse, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesKnowledge> arr = Business.Do<IExamQues>().KnlPager(orgid, pid, isuse, isdeleted, name, size, index, out count);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个机构下的专业，用于前端展示，被禁用的专业不显示
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        [Cache]
        public JArray KnlTreeFront(int orgid)
        {
            List<Song.Entities.QuesKnowledge> sbjs = Business.Do<IExamQues>().KnlCount(orgid, string.Empty, true, false, -1, -1);
            return sbjs.Count > 0 ? _KnlsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 机构下的专业，树形数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public JArray KnlTree(int orgid, string search, bool? isuse)
        {
            List<Song.Entities.QuesKnowledge> sbjs = Business.Do<IExamQues>().KnlCount(orgid, search, isuse, false, -1, -1);
            return sbjs.Count > 0 ? _KnlsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _KnlsNode(Song.Entities.QuesKnowledge item, List<Song.Entities.QuesKnowledge> items)
        {
            List<Song.Entities.QuesKnowledge> childs = new List<Song.Entities.QuesKnowledge>();
            for (int i = 0; i < items.Count; i++)
            {
                Entities.QuesKnowledge m = items[i];
                if (item == null && m.Qk_PID != 0) continue;
                if (item != null && m.Qk_PID != item.Qk_ID) continue;
                childs.Add(m);
                items.RemoveAt(i);
                i--;
            }
            JArray jarr = new JArray();
            for (int i = 0; i < childs.Count; i++)
            {
                string j = childs[i].ToJson("", "Qk_CrtTime,Qk_UpdateTime");
                JObject jo = JObject.Parse(j);
                jarr.Add(jo);
                //计算下级
                JArray charray = _KnlsNode(childs[i], items);
                if (charray.Count > 0) jo.Add("children", charray);
            }
            return jarr;
        }
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">专业的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool KnlModifyTaxis(Song.Entities.QuesKnowledge[] list)
        {
            try
            {
                Business.Do<IExamQues>().KnlUpdateTaxis(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            => Business.Do<IExamQues>().KnlQusTotal(orgid, qkid, qtype, isUse, children);
        #endregion

        #region 试题关键字
        /// <summary>
        /// 获取试题关键字的单条数据
        /// </summary>
        /// <param name="id">试题知识点id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesTags TagForID(long id)
        {
            return Business.Do<IExamQues>().TagSingle(id);
        }
        /// <summary>
        /// 添加试题关键字
        /// </summary>
        /// <param name="entity">试题关键字的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public int TagAdd(Song.Entities.QuesTags entity)
        {
            int count = 0;
            entity.Qtag_Name = entity.Qtag_Name.Replace(",", " ").Replace("，", " ");
            foreach (string name in entity.Qtag_Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Song.Entities.QuesTags tag = new QuesTags();
                tag.Qtag_Name = name;
                tag.Qtag_Weight = entity.Qtag_Weight;
                tag.Qtag_IsDeleted = entity.Qtag_IsDeleted;
                tag.Org_ID = entity.Org_ID;
                tag.Cou_ID = entity.Cou_ID;
                count += Business.Do<IExamQues>().TagAdd(tag);
            }
            return count;
        }
        /// <summary>
        /// 修改试题知识点
        /// </summary>
        /// <param name="entity">试题知识点的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public Song.Entities.QuesTags TagModify(Song.Entities.QuesTags entity)
        {
            Song.Entities.QuesTags old = Business.Do<IExamQues>().TagSingle(entity.Qtag_ID);
            if (old == null) throw new Exception("Not found entity for QuesTags！");
            try
            {
                old.Copy<Song.Entities.QuesTags>(entity);
                Business.Do<IExamQues>().TagSave(old);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int TagDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题知识点
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int TagRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int TagRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题知识点
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid">课程id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult TagPager(int orgid, long couid, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesTags> arr = Business.Do<IExamQues>().TagPager(orgid, couid, isdeleted, name, size, index, out count);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签的id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public int TagQusTotal(long qtagid, long couid, int qtype, bool? isuse)
            => Business.Do<IExamQues>().TagQusTotal(qtagid, couid, qtype, isuse);
        #endregion
    }
}
