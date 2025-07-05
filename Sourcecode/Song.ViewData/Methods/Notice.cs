﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 通知公告
    /// </summary>
    [HttpPut, HttpGet]
    public class Notice : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 通过id获取通知信息
        /// </summary>
        /// <param name="id">通知id</param>
        /// <returns>实体</returns>
        public Song.Entities.Notice ForID(long id)
        {
            return Business.Do<INotice>().NoticeSingle(id);
        }
        /// <summary>
        ///  通过id获取通知信息，前端获取，如果通知禁止显示则不返回
        /// </summary>
        /// <param name="id">通知id</param>
        /// <returns>实体</returns>
        public Song.Entities.Notice ShowForID(long id)
        {
            Song.Entities.Notice notice= Business.Do<INotice>().NoticeSingle(id);
            return notice.No_IsShow ? notice : null;
        }
        /// <summary>
        /// 获取或增加通知的浏览数
        /// </summary>
        /// <param name="id">通知ID</param>
        /// <param name="num">要增总的浏览数，不可以为负数或零</param>
        /// <returns></returns>    
        public int ViewNum(long id, int num)
        {
            return Business.Do<INotice>().ViewNum(id, num);
        }
        /// <summary>
        /// 修改通知信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(Song.Entities.Notice entity)
        {
            Song.Entities.Notice old = Business.Do<INotice>().NoticeSingle(entity.No_Id);
            if (old == null) throw new Exception("Not found entity for Notice！");

            old.Copy<Song.Entities.Notice>(entity);
            Business.Do<INotice>().Save(old);
            return true;
        }
        /// <summary>
        /// 添加通知信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Add(Song.Entities.Notice entity)
        {
            Business.Do<INotice>().Add(entity);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<INotice>().Delete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ModifyOrder(Song.Entities.Notice[] items)
        {
            Business.Do<INotice>().UpdateOrder(items);
            return true;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">按标题检索的字符</param>
        /// <param name="iscontext">是否输出内容，有时为了方便列表展示，可以不输出通知的内容部分，以方便快速加载</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, string search,bool iscontext, int size, int index)
        {            
            int count = 0;
            Song.Entities.Notice[] eas = Business.Do<INotice>().GetPager(orgid, null, search, size, index, out count);
            if (!iscontext) 
                foreach (Song.Entities.Notice e in eas) e.No_Context = "";
            
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取供前端显示的通知公告
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ShowPager(int orgid, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Notice[] notices = Business.Do<INotice>().GetPager(orgid, true, search, size, index, out count);
            ListResult result = new ListResult(notices);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 作为弹窗广告的通知
        /// </summary>
        /// <param name="forpage">弹出的页面</param>
        /// <returns></returns>
        public ListResult OpenItems(string forpage)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Notice[] eas = Business.Do<INotice>().List(org.Org_ID, 2, forpage, DateTime.Now, true, -1);
            ListResult result = new ListResult(eas);
            result.Index = 1;
            result.Size = eas.Length;
            result.Total = eas.Length;
            return result;
        }
        /// <summary>
        /// 显示通知公告
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">1为普通通知，2为弹窗通知，-1取所有</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Song.Entities.Notice[] ShowItems(int orgid, int type, int count)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            return Business.Do<INotice>().GetCount(orgid, type, true, count);
        }


    }
}
