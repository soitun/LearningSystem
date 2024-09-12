using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ѧ�ƹ���
    /// </summary>
    public interface ISubject : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���ѧ����רҵ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int SubjectAdd(Subject entity);
        /// <summary>
        /// �������רҵ�������ڵ���ʱ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="names">רҵ���ƣ��������ö��ŷָ��Ķ������</param>
        /// <returns></returns>
        Subject SubjectBatchAdd(int orgid, string names);
        /// <summary>
        /// �Ƿ��Ѿ�����רҵ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Subject SubjectIsExist(int orgid, long pid, string name);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SubjectSave(Subject entity);
        /// <summary>
        /// �޸�רҵ��ĳЩ��
        /// </summary>
        /// <param name="sbjid">רҵid</param>
        /// <param name="fields">�ֶ�</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool SubjectUpdate(long sbjid, Field[] fields, object[] objs);
        /// <summary>
        /// �޸�רҵ��ĳЩ��
        /// </summary>
        /// <param name="sbjid">רҵid</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool SubjectUpdate(long sbjid, Field field, object obj);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void SubjectDelete(long identify);
        /// <summary>
        /// ���רҵ�µ���������
        /// </summary>
        /// <param name="identify"></param>
        void SubjectClear(long identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Subject SubjectSingle(long identify);
        /// <summary>
        /// ��ǰרҵ�µ�������רҵid
        /// </summary>
        /// <param name="sbjid">��ǰרҵid</param>
        /// <param name="orgid">רҵ����������ID,���С�ڵ����㣬��ȡ�����ݿ��ȡsbjid��ȡorgid�����Խ�����ȷ��ֵ�����Լ������ݿ��ȡ����</param>
        List<long> TreeID(long sbjid, int orgid);
        /// <summary>
        /// ��ȡרҵ���ƣ����Ϊ�༶������ϸ�������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string SubjectName(long identify);
        /// <summary>
        /// ��ǰרҵ���Ƿ�����רҵ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify">��ǰרҵId</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <returns>���Ӽ�������true</returns>
        bool SubjectIsChildren(int orgid, long identify, bool? isUse);
        
        /// <summary>
        /// ��ȡѧ��/רҵ
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="sear">�����ؼ���</param>
        /// <param name="isUse"></param>
        /// <param name="pid">�ϼ�ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Subject> SubjectCount(int orgid, string sear, bool? isUse, long pid, int count);
        /// <summary>
        /// ȡָ��������ѧ�ƻ�רҵ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="pid"></param>
        /// <param name="order">����ʽ��defĬ���������Ƽ���������ţ���tax�������,rec���Ƽ�</param>
        /// <param name="index">��ʼ����</param>
        /// <param name="size">ȡ������</param>
        /// <returns></returns>
        List<Subject> SubjectCount(int orgid, string sear, bool? isUse, long pid, string order, int index, int size);
        /// <summary>
        /// ��ȡѧ��/רҵ
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="sear">�����ؼ���</param>
        /// <param name="isUse"></param>
        /// <param name="pid">�ϼ�ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Subject> SubjectCount(int orgid, int depid, string sear, bool? isUse, long pid, int count);
        /// <summary>
        /// ��ǰרҵ���ϼ�����
        /// </summary>
        /// <param name="sbjid"></param>
        /// <param name="isself">�Ƿ��������</param>
        /// <returns></returns>
        List<Subject> Parents(long sbjid, bool isself);
        /// <summary>
        /// ����רҵ����
        /// </summary>
        /// <param name="orgid">����id</param>       
        /// <param name="pid">�ϼ�id</param>
        /// <param name="isUse">�Ƿ����õģ�nullȡ����</param>
        /// <param name="children">�Ƿ�����Ӽ�</param>
        /// <returns></returns>
        int SubjectOfCount(int orgid, long pid, bool? isUse,bool children);
        /// <summary>
        /// ����רҵ��ͳ������
        /// </summary>
        /// <param name="orgid">����id���������0����ˢ�µ�ǰ�����µ�����רҵ����</param>
        /// <param name="sbjid">רҵid</param>
        /// <returns></returns>
        bool UpdateStatisticalData(int orgid, long sbjid);
        /// <summary>
        /// ��ǰѧ���µ���������
        /// </summary>
        /// <param name="orgid">��ǰ����</param>
        /// <param name="sbjid"></param>
        /// <param name="qusType">��������</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> QusForSubject(int orgid, long sbjid, int qusType, bool? isUse, int count);
        /// <summary>
        /// ��ȡרҵ���µ���������
        /// </summary>
        /// <param name="orgid">��ǰ����</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="qusType">�������</param>
        /// <param name="isUse">�Ƿ����õ�����</param>
        /// <returns></returns>
        int QusCountForSubject(int orgid, long sbjid, int qusType, bool? isUse);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">�ϼ�id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Subject> SubjectPager(int orgid, long pid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ����רҵ������
        /// </summary>
        /// <param name="list">רҵ�б�������ֻ��Sbj_ID��Sbj_PID��Sbj_Tax��Sbj_Level</param>
        /// <returns></returns>
        bool UpdateTaxis(Subject[] list);

    }
}
