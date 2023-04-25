using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Resources;
using System.Reflection;

namespace Song.ServiceImpls
{
    public partial class ContentsCom : IContents
    {
        private string _artUppath = "News";
        public int ArticleAdd(Article entity)
        {
            if (entity.Art_ID <= 0)
                entity.Art_ID = WeiSha.Core.Request.SnowID();
            //����ʱ��
            entity.Art_CrtTime = DateTime.Now;
            if (entity.Art_PushTime < DateTime.Now.AddYears(-100))
                entity.Art_PushTime = entity.Art_CrtTime;
            //���ڻ���
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //������Ŀ
            Song.Entities.Columns nc = Business.Do<IColumns>().Single(entity.Col_UID);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //������ö�����
            if (entity.Art_IsTop)
            {
                int topNumber = Business.Do<ISystemPara>()["NewsMaxTop"].Int16 ?? 10;
                int count = Gateway.Default.Count<Article>(Article._.Art_IsTop == true);
                //����ö���Ϣ����
                if (count >= topNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsTop == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(count - topNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsTop = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //������Ƽ�������
            if (entity.Art_IsRec)
            {
                int recNumber = Business.Do<ISystemPara>()["NewsMaxRec"].Int16 ?? 10;
                int reccount = Gateway.Default.Count<Article>(Article._.Art_IsRec == true);
                //����ö���Ϣ����
                if (reccount >= recNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsRec == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(reccount - recNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsRec = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //�������Ҫ���
            bool isveri = Business.Do<ISystemPara>()["NewsIsVerify"].Boolean ?? true;
            if (!isveri)
            {
                entity.Art_IsVerify = true;
            }
            entity.Art_IsUse = true;
            return Gateway.Default.Save<Article>(entity);
        }

        public void ArticleSave(Article entity)
        {
            entity.Art_LastTime = DateTime.Now;
            if (entity.Art_PushTime < DateTime.Now.AddYears(-100))
                entity.Art_PushTime = entity.Art_CrtTime;
            Song.Entities.Columns nc = Business.Do<IColumns>().Single(entity.Col_UID);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //������ö�����
            if (entity.Art_IsTop)
            {
                int topNumber = Business.Do<ISystemPara>()["NewsMaxTop"].Int16 ?? 10;
                int count = Gateway.Default.Count<Article>(Article._.Art_IsTop == true);
                //����ö���Ϣ����
                if (count >= topNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsTop == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(count - topNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsTop = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //������Ƽ�������
            if (entity.Art_IsRec)
            {
                int recNumber = Business.Do<ISystemPara>()["NewsMaxRec"].Int16 ?? 10;
                int reccount = Gateway.Default.Count<Article>(Article._.Art_IsRec == true);
                //����ö���Ϣ����
                if (reccount >= recNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsRec == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(reccount - recNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsRec = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //�������Ҫ���
            bool isveri = Business.Do<ISystemPara>()["NewsIsReVeri"].Boolean ?? true;
            if (!isveri)
            {
                entity.Art_IsVerify = true;
            }
            //����޸ĺ���Ҫ�������
            bool isrevi = Business.Do<ISystemPara>()["NewsIsReVeri"].Boolean ?? true;
            if (isveri)
            {
                entity.Art_IsVerify = false;
            }
            Gateway.Default.Save<Article>(entity);
        }

        /// <summary>
        /// �޸��������µ�״̬
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool ArticleUpdate(long artid, Field[] fiels, object[] objs)
        {
            try
            {
                Gateway.Default.Update<Article>(fiels, objs, Article._.Art_ID == artid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ArticleAddNumber(long id, int addNum)
        {
            object obj = Gateway.Default.Max<Article>(Article._.Art_Number, Article._.Art_ID == id);
            int i = 0;
            try
            {
                i = (int)obj;
            }
            catch { }
            //����������ӣ���ֱ�ӷ��ص�ǰ�����
            if (addNum < 1) return i;

            //���Ӽ���
            i += addNum;
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_Number }, new object[] { i }, Article._.Art_ID == id);
            return i;
        }

        public void ArticleDelete(Article entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                ArticleDelete(entity, null);
            }       
        }
        public void ArticleDelete(Article entity, DbTrans tran)
        {
            if (tran == null) tran = Gateway.Default.BeginTrans();
            try
            {
                //ɾ������
                Business.Do<IAccessory>().Delete(entity.Art_Uid, string.Empty);
                tran.Delete<Article>(Article._.Art_ID == entity.Art_ID);
                //ɾ��ͼƬ�ļ�
                string img = WeiSha.Core.Upload.Get[_artUppath].Physics + entity.Art_Logo;
                if (System.IO.File.Exists(img))
                    System.IO.File.Delete(img);
                //ɾ������
                tran.Delete<Article>(Article._.Art_ID == entity.Art_ID);
                WeiSha.Core.Upload.Get[_artUppath].DeleteDirectory(entity.Art_ID.ToString());
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;

            }
            finally
            {
                tran.Close();
            }
        }
        public void ArticleDelete(long identify)
        {
            Article na = this.ArticleSingle(identify);
            this.ArticleDelete(na);
        }

        public void ArticleDeleteAll(int orgid, string coluid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(coluid)) wc.And(Article._.Col_UID == coluid);
            Song.Entities.Article[] entities = Gateway.Default.From<Article>().Where(wc).ToArray<Article>();
            foreach (Song.Entities.Article entity in entities)
            {
                ArticleDelete(entity);
            }
        }

        public void ArticleIsDelete(long identify)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsDel }, new object[] { true }, Article._.Art_ID == identify);
        }

        public void ArticleRecover(long identify)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsDel }, new object[] { false }, Article._.Art_ID == identify);
        }

        public void ArticlePassVerify(long identify, string verMan)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsVerify, Article._.Art_VerifyTime, Article._.Art_VerifyMan }, new object[] { true, DateTime.Now, verMan }, Article._.Art_ID == identify);
        }

        public Article ArticleSingle(long identify)
        {
            return Gateway.Default.From<Article>().Where(Article._.Art_ID == identify).ToFirst<Article>();
        }
        /// <summary>
        /// ��ǰ���ŵ���һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Article ArticlePrev(long identify, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Article._.Org_ID == orgid;
            wc &= Article._.Art_IsShow == true;
            wc &= Article._.Art_PushTime < DateTime.Now;
            Song.Entities.Article art = this.ArticleSingle(identify);
            return Gateway.Default.From<Article>().OrderBy(Article._.Art_PushTime.Asc)
                .Where(wc && Article._.Art_PushTime > art.Art_PushTime).ToFirst<Article>();
        }
        /// <summary>
        /// ��ǰ���ŵ���һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Article ArticleNext(long identify, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Article._.Org_ID == orgid;
            wc &= Article._.Art_IsShow == true;
            wc &= Article._.Art_PushTime < DateTime.Now;
            Song.Entities.Article art = this.ArticleSingle(identify);
            return Gateway.Default.From<Article>().OrderBy(Article._.Art_PushTime.Desc)
                .Where(wc && Article._.Art_PushTime < art.Art_PushTime).ToFirst<Article>();
        }
        public Special[] Article4Special(long identify)
        {
            return Gateway.Default.From<Special>().InnerJoin<Special_Article>(Special_Article._.Sp_Id == Special._.Sp_Id)
                .Where(Special_Article._.Art_Id == identify).ToArray<Special>();
        }

        public Article[] ArticleCount(int orgid, string coluid, int topNum, string order)
        {
            return this.ArticleCount(orgid, coluid, topNum, null, order);
        }
        public Article[] ArticleCount(int orgid, string coluid, int topNum, bool? isuse, string order)
        {
            WhereClip wc = Article._.Art_IsDel == false;
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (isuse != null) wc.And(Article._.Art_IsUse == (bool)isuse);
            if (!string.IsNullOrWhiteSpace(coluid))
            {
                WhereClip wcColid = new WhereClip();
                List<string> list = Business.Do<IColumns>().TreeID(coluid);
                foreach (string l in list)
                    wcColid.Or(Article._.Col_UID == l);
                wc.And(wcColid);
            }
            OrderByClip wcOrder = new OrderByClip();
            if (order == "top") wcOrder = Article._.Art_IsTop.Desc;
            if (order == "hot") wcOrder = Article._.Art_IsHot.Desc;
            if (order == "img") wcOrder = Article._.Art_IsImg.Desc;
            if (order == "rec") wcOrder = Article._.Art_IsRec.Desc;
            if (order == "flux") wcOrder = Article._.Art_Number.Desc;
            Song.Entities.Article[] arts = Gateway.Default.From<Article>().Where(wc).OrderBy(wcOrder && Article._.Art_PushTime.Desc && Article._.Art_CrtTime.Desc).ToArray<Article>(topNum);
            return arts;
        }
        /// <summary>
        /// ͳ����������
        /// </summary>
        /// <param name="coluid">��Ŀuid</param>
        /// <param name="isuse">�Ƿ����õ�</param>
        /// <returns></returns>
        public int ArticleOfCount(int orgid, string coluid, bool? isuse)
        {
            //WhereClip wc = Article._.Art_IsDel == false && Article._.Art_IsShow == true && Article._.Art_IsVerify == true;
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (isuse != null) wc.And(Article._.Art_IsUse == (bool)isuse);
            if (!string.IsNullOrWhiteSpace(coluid))
            {
                WhereClip wcColid = new WhereClip();
                List<string> list = Business.Do<IColumns>().TreeID(coluid);
                foreach (string l in list)
                    wcColid.Or(Article._.Col_UID == l);
                wc.And(wcColid);
            }
            //if (!string.IsNullOrWhiteSpace(coluid)) wc.And(Article._.Col_UID == coluid);
            return Gateway.Default.Count<Article>(wc);
        }

        public Article[] ArticlePager(int orgid, string coluid, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Article._.Art_IsDel == false;
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(coluid))
            {
                WhereClip wcColid = new WhereClip();
                List<string> list = Business.Do<IColumns>().TreeID(coluid);
                foreach (string l in list)
                    wcColid.Or(Article._.Col_UID == l);
                wc.And(wcColid);
            }
            if (isShow != null) wc.And(Article._.Art_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                  wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
              countSum = Gateway.Default.Count<Article>(wc);
              return Gateway.Default.From<Article>().Where(wc).OrderBy(Article._.Art_PushTime.Desc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] ArticlePager(int orgid, string coluid, bool? isVerify, bool? isuse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(coluid))
            {
                WhereClip wcColid = new WhereClip();
                List<string> list = Business.Do<IColumns>().TreeID(coluid);
                foreach (string l in list)
                    wcColid.Or(Article._.Col_UID == l);
                wc.And(wcColid);
            }
            if (searTxt != null && searTxt.Trim() != "") wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            if (isVerify != null) wc.And(Article._.Art_IsVerify == (bool)isVerify);
            if (isuse != null) wc.And(Article._.Art_IsUse == (bool)isuse);
            countSum = Gateway.Default.Count<Article>(wc);
            return Gateway.Default.From<Article>().Where(wc).OrderBy(Article._.Art_PushTime.Desc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] ArticlePager(int orgid, string coluid, string searTxt, bool? isVerify, bool? isuse, string order,int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(coluid))
            {
                WhereClip wcColid = new WhereClip();
                List<string> list = Business.Do<IColumns>().TreeID(coluid);
                foreach (string l in list)
                    wcColid.Or(Article._.Col_UID == l);
                wc.And(wcColid);
            }
            if (searTxt != null && searTxt.Trim() != "") wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            if (isVerify != null) wc.And(Article._.Art_IsVerify == (bool)isVerify);
            if (isuse != null) wc.And(Article._.Art_IsUse == (bool)isuse);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "top") wcOrder = Article._.Art_IsTop.Desc;
            if (order == "hot") wcOrder = Article._.Art_IsHot.Desc;
            if (order == "img") wcOrder = Article._.Art_IsImg.Desc;
            if (order == "rec") wcOrder = Article._.Art_IsRec.Desc;
            if (order == "flux") wcOrder = Article._.Art_Number.Desc;
            countSum = Gateway.Default.Count<Article>(wc);
            return Gateway.Default.From<Article>().Where(wc).OrderBy(wcOrder && Article._.Art_ID.Desc).ToArray<Article>(size, (index - 1) * size);
        }

    }
}
