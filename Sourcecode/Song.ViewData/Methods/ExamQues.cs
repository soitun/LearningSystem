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

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试专用题库的管理，有区别于课程中的题库
    /// </summary>
    public class ExamQues : ViewMethod, IViewAPI
    {
        
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
        public bool CollectAdd(int accid, long qusid) => Business.Do<IExamQues>().CollectAdd(accid, qusid);
        /// <summary>
        /// 取消试题收藏
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public bool CollectRemove(int accid, long qusid) => Business.Do<IExamQues>().CollectRemove(accid, qusid);

        /// <summary>
        /// 收藏状态的变更
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <param name="state">收藏状态，为true时创建收藏关联，否则删除收藏</param>
        /// <returns></returns>
        public bool CollectUpdate(int accid, long qusid,bool state)
        {
            if(state) return Business.Do<IExamQues>().CollectAdd(accid, qusid);
            else return Business.Do<IExamQues>().CollectRemove(accid, qusid);
        }


        #endregion
    }
}
