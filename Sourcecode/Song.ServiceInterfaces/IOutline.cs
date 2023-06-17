using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �½ڹ���
    /// </summary>
    public interface IOutline : WeiSha.Core.IBusinessInterface
    {
        #region �½ڹ���
        /// <summary>
        /// ����½�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        Outline OutlineAdd(Outline entity);
        /// <summary>
        /// ��������½ڣ������ڵ���ʱ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ̣���γ̣�id</param>
        /// <param name="names">���ƣ��������ö��ŷָ��Ķ������</param>
        /// <returns></returns>
        Outline OutlineBatchAdd(int orgid, long  sbjid, long couid, string names);
        /// <summary>
        /// �Ƿ��Ѿ������½�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ̣���γ̣�id</param>
        /// <param name="pid">�ϼ�id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Outline OutlineIsExist(int orgid, long  sbjid, long couid, long pid, string name);
        /// <summary>
        /// �޸��½�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void OutlineSave(Outline entity);
        /// <summary>
        /// �����½ڵ�������
        /// </summary>
        /// <param name="olid">�½�Id</param>
        /// <param name="count">������</param>
        /// <returns></returns>
        int UpdateQuesCount(long olid, int count);
        /// <summary>
        /// �����½ڣ�����ʱ���������ɻ���
        /// </summary>
        /// <param name="entity"></param>
        void OutlineInput(Outline entity);
        /// <summary>
        /// �����γ��½ڵ�Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="couid">�γ�ID</param>
        /// <returns></returns>
        string OutlineExport4Excel(string path, long couid);
        /// <summary>
        /// ɾ���½�
        /// </summary>
        /// <param name="entity">�½ڶ���</param>
        /// <param name="freshCache"></param>
        void OutlineDelete(Outline entity, bool freshCache);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="olid">ʵ�������</param>
        void OutlineDelete(long olid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="olid">ʵ�������</param>
        /// <returns></returns>
        Outline OutlineSingle(long olid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�Ψһֵ����UID��
        /// </summary>
        /// <param name="uid">ȫ��Ψһֵ</param>
        /// <returns></returns>
        Outline OutlineSingle(string uid);
        /// <summary>
        /// ��ȡĳ���γ��ڵ��½ڣ�������ȡ
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <param name="names">�༶����</param>
        /// <returns></returns>
        Outline OutlineSingle(long couid, List<string> names);
        /// <summary>
        /// ��ǰ�½��µ��������½�id
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        List<long> TreeID(long olid);
        /// <summary>
        /// ��ȡ�½����ƣ����Ϊ�༶������ϸ�������
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        string OutlineName(long olid);
        /// <summary>
        /// ��ȡ���пγ��½�
        /// </summary>
        /// <param name="couid">�����γ�id</param>
        /// <param name="use">�Ƿ�����</param>
        /// <param name="finish">�½��Ƿ����</param>
        /// <param name="video">�Ƿ�Ϊ��Ƶ�½�</param>
        /// <returns></returns>
        List<Outline> OutlineAll(long couid, bool? use, bool? finish, bool? video);
        /// <summary>
        /// �������棬�½ڻ����Կγ�Ϊ��λ�洢
        /// </summary>
        List<Outline> BuildCache(long couid);
        /// <summary>
        /// �������νṹ���½��б�
        /// </summary>
        /// <param name="outlines"></param>
        /// <returns></returns>
        DataTable OutlineTree(Song.Entities.Outline[] outlines);
        /// <summary>
        /// ����½�������͸���
        /// </summary>
        /// <param name="olid"></param>
        void OutlineClear(long olid);
        /// <summary>
        /// ����½�������͸���
        /// </summary>
        /// <param name="entity">�½ڶ���</param>
        /// <param name="freshCache"></param>
        void OutlineClear(Outline entity, bool freshCache);
        /// <summary>
        /// ������Ч�½�
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <returns></returns>
        int OutlineCleanup(long couid);
        ///// <summary>
        ///// ��������
        ///// </summary>
        ///// <returns></returns>
        //List<Outline> OutlineBuildCache();
        /// <summary>
        /// ��ȡָ���������½��б�
        /// </summary>
        /// <param name="couid">�����γ�id</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count">ȡ��������¼�����С�ڵ���0����ȡ����</param>
        /// <returns></returns>
        List<Outline> OutlineCount(long couid, string search, bool? isUse, int count);
        /// <summary>
        /// ȡָ���������½�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid">��id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Outline> OutlineCount(long couid, long pid, bool? isUse, int count);
        /// <summary>
        /// ��ȡָ���������½��б�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="islive">�Ƿ���ֱ���½�</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Outline> OutlineCount(long couid, long pid, bool? islive, string search, bool? isUse, int count);
        /// <summary>
        /// ȡָ���������½�
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="pid">�½��ϼ�Id</param>
        /// <param name="islive">�Ƿ���ֱ���½�</param>
        /// <param name="search">���½����Ƽ���</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Outline> OutlineCount(int orgid, long sbjid, long couid, long pid, bool? islive, string search, bool? isUse, int count);
        /// <summary>
        /// ֱ���е��½�
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Outline> OutlineLiving(int orgid, int count);         
        /// <summary>
        /// ��ǰ�γ��µ��½���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse);
        /// <summary>
        /// ��ǰ�γ��µ��½���
        /// </summary>
        /// <param name="couid">�γ�id<</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <param name="children">�Ƿ��¼�</param>
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse, bool children);
        /// <summary>
        /// ��ǰ�γ��µ��½���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="pid">��id</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="isVideo">�Ƿ�����Ƶ</param>
        /// <param name="isFinish">�½��Ƿ����</param>
        /// <param name="children">�Ƿ��¼�</param> 
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse, bool? isVideo, bool? isFinish, bool? children);
        /// <summary>
        /// �Ƿ����Ӽ��½�
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="pid">��id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsChildren(long couid, long pid, bool? isUse);
        /// <summary>
        /// ��ǰ�½��Ƿ�������
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsQues(long olid, bool? isUse);
        /// <summary>
        /// ��ǰ�½ڵ��Ӽ��½�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Outline[] OutlineChildren(long couid, long pid, bool? isUse, int count);
        /// <summary>
        /// ��ҳȡ�γ��½ڵ���Ϣ
        /// </summary>
        /// <param name="couid">�����γ�id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Outline[] OutlinePager(long couid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ�½ڵ�����
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesCount(long olid, int type, bool? isUse, int count);        
        /// <summary>
        /// ��ǰ�½��ж��ٵ�����
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="isAll">�Ƿ�ȡ���У���ǰ�½����������½ڵ�����һ���㣩</param>
        /// <returns></returns>
        int QuesOfCount(long olid, int type, bool? isUse, bool isAll);        
        /// <summary>
        /// �����½ڵ�����
        /// </summary>
        /// <param name="list">רҵ�б�Ol_ID��Ol_PID��Ol_Tax��Ol_Level</param>
        /// <returns></returns>
        bool UpdateTaxis(Outline[] list);
        #endregion

        #region ����ͳ��
        /// <summary>
        /// ͳ�������½�����
        /// </summary>
        /// <returns></returns>
        int StatisticalQuestion();
        /// <summary>
        /// ͳ��ĳ���½ڵ�����
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        int StatisticalQuestion(long olid);
        /// <summary>
        /// ͳ��ָ���½ڵ�����
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        int StatisticalQuestion(long[] olid);
        #endregion

        #region �½��¼�
        /// <summary>
        /// ����½�����Ƶ�����¼�
        /// </summary>
        /// <param name="entity"></param>
        void EventAdd(OutlineEvent entity);
        /// <summary>
        /// �޸Ĳ����¼�
        /// </summary>
        /// <param name="entity"></param>
        void EventSave(OutlineEvent entity);
        /// <summary>
        /// ɾ���¼�
        /// </summary>
        /// <param name="entity"></param>
        void EventDelete(OutlineEvent entity);
        /// <summary>
        /// ɾ���¼�
        /// </summary>
        /// <param name="identify"></param>
        void EventDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        OutlineEvent EventSingle(int identify);
        /// <summary>
        /// �����½��������¼�
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <param name="olid">�½�ID��������Ϊ�㣬�����ȡ����</param>
        /// <param name="type">�¼����ͣ�1Ϊ���ѣ�2Ϊ֪ʶչʾ��3�������ʣ�4ʵʱ���������磬ѡ��ĳ�����ת��ĳ�룩</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        OutlineEvent[] EventAll(long couid, long olid, int type, bool? isUse);
        /// <summary>
        /// �����½��������¼�
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <param name="uid">�½ڵ�ȫ��Ψһֵ</param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        OutlineEvent[] EventAll(long couid, string uid, int type, bool? isUse);
        /// <summary>
        /// ��ȡ�������͵���Ϣ
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        DataTable EventQues(int oeid);
        /// <summary>
        /// ��ȡʱ�䷴������Ϣ
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        DataTable EventFeedback(int oeid);
        #endregion
    }
}
