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

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试专用题库的管理，有区别于课程中的题库
    /// </summary>
    public class ExamQues : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 获取题库列表
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, int index, int size)
        {
            int sum = 0;
            List<Questions> list = Business.Do<IExamQues>().Pager(orgid, null, null, null, null, null, size, index, out sum);
           
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
            int sum = 0;
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
        /// 删除试题分类，下级分类也会一并删除
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
        #endregion
    }
}
