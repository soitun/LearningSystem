using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ��վ���ݵĹ���
    /// </summary>
    public interface IContents : WeiSha.Core.IBusinessInterface
    {
        #region �������µĹ���
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int ArticleAdd(Article entity);
        /// <summary>
        /// �޸���������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ArticleSave(Article entity);
        /// <summary>
        /// �޸��������µ�״̬
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool ArticleUpdate(long artid, Field[] fiels, object[] objs);
        /// <summary>
        /// ʹ��ǰ�������������һ��������id�������������Ч�ʸ���
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="id">�������µ�id</param>
        /// <param name="addNum">ÿ��������Ӽ�����</param>
        /// <returns></returns>
        int ArticleAddNumber(long artid, int addNum);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ArticleDelete(Article entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        /// <param name="tran">�������</param>
        void ArticleDelete(Article entity, DbTrans tran);
        /// <summary>
        /// ����ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void ArticleDelete(long identify);
        /// <summary>
        /// ɾ��������������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="coluid">��Ŀuid</param>
        void ArticleDeleteAll(int orgid, string coluid);
        /// <summary>
        /// ��׼�����Ƿ���ɾ��״̬�����������վ
        /// </summary>
        /// <param name="identify"></param>
        void ArticleIsDelete(long identify);
        /// <summary>
        /// ���»�ԭ���ӻ���վ�ص������б�
        /// </summary>
        /// <param name="identify"></param>
        void ArticleRecover(long identify);
        /// <summary>
        /// ͨ�����
        /// </summary>
        /// <param name="identify">����id</param>
        /// <param name="verMan">�����</param>
        void ArticlePassVerify(long identify, string verMan);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Article ArticleSingle(long identify);
        /// <summary>
        /// ��ǰ���ŵ���һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Article ArticlePrev(long identify, int orgid);
        /// <summary>
        /// ��ǰ���ŵ���һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Article ArticleNext(long identify, int orgid);
        /// <summary>
        /// ��ǰ�������ڵ�ר��
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Special[] Article4Special(long identify);
        /// <summary>
        /// ��������Ŀ��ȡ��������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="colId">��Ŀid,���idС��0����ȡȫ��</param>
        /// <param name="topNum">��ȡ��¼��</param>
        /// <param name="order">��ȡ���Ĭ��nullȡ�����ö������ȣ�hot�ȵ����ȣ�flux�����������,imgΪͼƬ����</param>
        /// <returns></returns>
        Article[] ArticleCount(int orgid, string coluid, int topNum, string order);
        Article[] ArticleCount(int orgid, string coluid, int topNum, bool? isuse, string order);
        /// <summary>
        /// ͳ����������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="coluid">��Ŀuid</param>
        /// <param name="isuse">�Ƿ����õ�</param>
        /// <returns></returns>
        int ArticleOfCount(int orgid, string coluid, bool? isuse);
        /// <summary>
        /// ��ҳ��ȡ����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="coluid">��Ŀid,���idС��0����ȡȫ��<</param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="searTxt">���������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ����Ŀ�����⣬�Ƿ��������ҳ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="coluid"></param>
        /// <param name="isVerify">�Ƿ����</param>
        /// <param name="isuse">�Ƿ�����</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, bool? isVerify, bool? isuse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="coluid">������Ŀ</param>
        /// <param name="searTxt">������ļ������ַ���</param>
        /// <param name="isVerify">�Ƿ����</param>
        /// <param name="isuse">�Ƿ�����</param>
        /// <param name="order">��ȡ���Ĭ��nullȡ�����ö������ȣ�hot�ȵ����ȣ�flux�����������,imgΪͼƬ����</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, string searTxt, bool? isVerify, bool? isuse, string order, int size, int index, out int countSum);
        #endregion

        #region ����ר�����
        /// <summary>
        /// �������ר��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int SpecialAdd(Special entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SpecialSave(Special entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SpecialDelete(Special entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void SpecialDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Special SpecialSingle(int identify);
        /// <summary>
        /// ��ǰר����Ͻ������
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="searTxt"></param>
        /// <returns></returns>
        Article[] Special4Article(int identify, string searTxt);
        /// <summary>
        /// ��ǰר����Ͻ������
        /// </summary>
        /// <param name="identify">ר��id</param>
        /// <param name="searTxt">��������Ϣ</param>
        /// <param name="num">ȡ������</param>
        /// <param name="type">��ȡ���Ĭ��nullȡ�����ö������ȣ�hot�ȵ����ȣ�maxFlux�����������</param>
        /// <returns></returns>
        Article[] Special4Article(int identify, string searTxt, int num, string type);
        /// <summary>
        /// ȡ����ר��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Special[] SpecialCount(int orgid, bool? isShow, bool? isUse, int count);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool SpecialUp(int orgid, int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool SpecialDown(int orgid, int id);
        /// <summary>
        /// ����ר�������µĹ���
        /// </summary>
        /// <param name="spId"></param>
        /// <param name="artId"></param>
        /// <returns></returns>
        bool SpecialAndArticle(int spId, int artId);
        /// <summary>
        /// ɾ��ר�������µĹ���
        /// </summary>
        /// <param name="spId"></param>
        /// <param name="artId"></param>
        /// <returns></returns>
        bool SpecialAndArticleDel(int spId, int artId);
        /// <summary>
        /// ר���б�
        /// </summary>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Special[] SpecialPager(string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ר���µ������б�
        /// </summary>
        /// <param name="spId">ר��id</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum);
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isShow, bool? isUse);
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isuse, bool? isShow, bool? isUse);
        Article[] SpecialArticle(int spId, string searTxt, int count);
        /// <summary>
        /// ר���µ�����
        /// </summary>
        /// <param name="spId">ר��Id</param>
        /// <param name="searTxt">�������ַ�</param>
        /// <param name="isuse">�Ƿ�ɾ��</param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="isUse">�Ƿ�ʹ��</param>
        /// <param name="count">ȡ��������С�ڵ���0��ȡ����</param>
        /// <param name="type">��ȡ���Ĭ��nullȡ�����ö������ȣ�hot�ȵ����ȣ�maxFlux�����������</param>
        /// <returns></returns>
        Article[] SpecialArticle(int spId, string searTxt, bool? isuse, bool? isShow, bool? isUse, int count, string type);
        #endregion

        #region �������۹���
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int NoteAdd(NewsNote entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NoteSave(NewsNote entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void NoteDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        NewsNote NoteSingle(int identify);
        /// <summary>
        /// ���ŵ�����
        /// </summary>
        /// <param name="artid">����id</param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="count"></param>
        /// <returns></returns>
        NewsNote[] NoteCount(int artid, bool? isShow, int count);
        /// <summary>
        /// ���µ�����
        /// </summary>
        /// <param name="artid">����id</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        NewsNote[] NotePager(int artid, string searTxt, bool? isShow, int size, int index, out int countSum);
        #endregion                         
     
        
    }
}
