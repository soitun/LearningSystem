using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ժϵְλ�Ĺ���
    /// </summary>
    public interface INotice : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        long Add(Notice entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(Notice entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(Notice entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(long identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Notice NoticeSingle(long identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰���������
        /// </summary>
        /// <param name="ttl">��������</param>
        /// <returns></returns>
        Notice NoticeSingle(string ttl);
        /// <summary>
        /// ��ǰ�������һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Notice NoticePrev(long identify, int orgid);
        /// <summary>
        /// ��ǰ�������һ������
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Notice NoticeNext(long identify, int orgid);
        /// <summary>
        /// ��ȡ���󣻼����й��棻
        /// </summary>
        /// <returns></returns>
        Notice[] GetAll();
        /// <summary>
        /// ��ȡ���й��棻
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <returns></returns>
        Notice[] GetAll(int orgid, bool? isShow);
        /// <summary>
        /// ��ȡָ�������ļ�¼
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type">1Ϊ��֪ͨͨ��2Ϊ����֪ͨ��-1ȡ����</param>
        /// <param name="isShow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Notice[] GetCount(int orgid, int type, bool? isShow, int count);
        /// <summary>
        /// ȡ���������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        int OfCount(int orgid, bool? isShow);
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num">Ҫ���ӵ�����</param>
        /// <returns></returns>
        int ViewNum(long id, int num);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="size">ÿҳ��ʾ������¼</param>
        /// <param name="index">��ǰ�ڼ�ҳ</param>
        /// <param name="countSum">��¼����</param>
        /// <returns></returns>
        Notice[] GetPager(int orgid, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="searTxt">��ѯ�ַ�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Notice[] GetPager(int orgid, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ȡ֪ͨ����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="type">1Ϊ��֪ͨͨ��2Ϊ����֪ͨ</param>
        /// <param name="forpage">��������ҳ</param>
        /// <param name="time">��ǰʱ��</param> 
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Notice[] List(int orgid, int type, string forpage, DateTime? time, bool? isShow, int count);

        /// <summary>
        /// <summary>
        /// ����˳��
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateTaxis(Notice[] items);
    }
}
