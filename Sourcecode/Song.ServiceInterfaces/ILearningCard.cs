using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ѧϰ������
    /// </summary>
    public interface ILearningCard : WeiSha.Core.IBusinessInterface
    {
        #region ѧϰ�����ù���
        /// <summary>
        /// ���ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SetAdd(LearningCardSet entity);
        /// <summary>
        /// �޸�ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <param name="scope">���ķ�Χ��1Ϊ����ʹ�õģ��Ѿ�ʹ�õĲ��ģ�2Ϊ����ȫ����Ĭ����1</param>
        void SetSave(LearningCardSet entity, int scope);
        /// <summary>
        /// ɾ��ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SetDelete(LearningCardSet entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void SetDelete(int identify);
        /// <summary>
        /// �ж�ѧϰ�������Ƿ��ظ�
        /// </summary>
        /// <param name="name">ѧϰ������</param>
        /// <param name="id">ѧϰ��id</param>
        /// <returns></returns>
        bool SetIsExist(string name, int id);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        LearningCardSet SetSingle(int identify);
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        LearningCardSet[] SetCount(int orgid, bool? isEnable, int count);
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        int SetOfCount(int orgid, bool? isEnable);
        /// <summary>
        /// ѧϰ������С���ȣ�ȡ����id�������ѧϰ������id���ֵ
        /// </summary>
        /// <returns></returns>
        int MinLength();
        /// <summary>
        /// ��ҳ��ȡѧϰ��������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum);
        #endregion

        #region �����γ�
        /// <summary>
        /// ��ȡѧϰ�������������Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        List<Course> CoursesGet(LearningCardSet set);
        /// <summary>
        ///  ��ȡѧϰ�������������Ŀγ�
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        List<Course> CoursesGet(string xml);
        /// <summary>
        /// ѧϰ�������Ŀγ�
        /// </summary>
        /// <param name="code">ѧϰ������</param>
        /// <param name="pw">ѧϰ������</param>
        /// <returns></returns>
        List<Course> CoursesForCard(string code, string pw);
        /// <summary>
        /// ���ù����Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet�����е�Lcs_RelatedCourses����¼������Ϣ</returns>
        LearningCardSet CoursesSet(LearningCardSet set, Course[] courses);
        LearningCardSet CoursesSet(LearningCardSet set, long[] couid);
        /// <summary>
        /// ���ù����Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <param name="couids">�γ�id�����Զ��ŷָ�</param>
        /// <returns></returns>
        LearningCardSet CoursesSet(LearningCardSet set, string  couids);
        #endregion

        #region ѧϰ������
        /// <summary>
        /// ���ɵ���ѧϰ������
        /// </summary>
        /// <param name="set">ѧϰ����������</param>
        /// <param name="code">����</param>
        /// <returns></returns>
        LearningCard CardGenerateObject(LearningCardSet set, string code);
        /// <summary>
        /// ��������ѧϰ��
        /// </summary>
        /// <param name="set">ѧϰ����������</param>        
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set);
        /// <summary>
        /// ��������ѧϰ��
        /// </summary>
        /// <param name="set"></param>
        /// <param name="tran">����</param>
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set, DbTrans tran);
        /// <summary>
        /// ����ѧϰ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardAdd(LearningCard entity);
        /// <summary>
        /// �޸�ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardSave(LearningCard entity);
        /// <summary>
        /// ɾ��ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardDelete(LearningCard entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void CardDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        LearningCard CardSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="code">ѧϰ������</param>
        /// <param name="pw">ѧϰ������</param>
        /// <returns></returns>
        LearningCard CardSingle(string code,string pw);
        /// <summary>
        /// У��ѧϰ���Ƿ���ڣ������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LearningCard CardCheck(string code);
        /// <summary>
        /// ѧϰ����ʹ������
        /// </summary>
        /// <param name="lscid">ѧϰ���������ID</param>
        /// <returns></returns>
        int CardUsedCount(int lscid);
        /// <summary>
        /// ʹ�ø�ѧϰ��
        /// </summary>
        /// <param name="card">ѧϰ��</param>
        /// <param name="acc">ѧԱ�˺�</param>
        void CardUse(LearningCard card, Accounts acc);
        /// <summary>
        /// ��ȡ��ѧϰ����ֻ���ݴ���ѧԱ�˻����£�����ʹ��
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="acc">ѧԱ�˺�</param>
        void CardReceive(LearningCard entity, Accounts acc);
        /// <summary>
        /// ѧϰ��ʹ�ú�Ļع�����ɾ��ѧԱ�Ĺ����γ�
        /// </summary>
        /// <param name="entity"></param>
        void CardRollback(LearningCard entity);
        /// <summary>
        /// ѧϰ��ʹ�ú�Ļع�����ɾ��ѧԱ�Ĺ����γ�
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isclear">�Ƿ�����ѧϰ��¼</param>
        void CardRollback(LearningCard entity,bool isclear);
        /// <summary>
        /// ѧϰ���������µ�����ѧϰ��
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="lcsid">ѧϰ���������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <returns></returns>
        List<LearningCard> CardCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, int count);
        /// <summary>
        /// ѧϰ���������µ�ѧϰ������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="lcsid">�����������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <param name="isback">�Ƿ񱻻ع�</param>
        /// <returns></returns>
        int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, bool? isback);
        /// <summary>
        /// ����Excel��ʽ��ѧϰ����Ϣ
        /// </summary>
        /// <param name="path">�����ļ���·�����������ˣ�</param>
        /// <param name="orgid">����id</param>
        /// <param name="rsid">ѧϰ���������id</param>
        /// <returns></returns>
        string Card4Excel(string path, int orgid, int rsid);
        /// <summary>
        /// ��ҳ��ȡѧϰ��������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="lcsid">ѧϰ���������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡѧϰ��������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="lcsid"></param>
        /// <param name="code"></param>
        /// <param name="account">ʹ�����˺�</param>
        /// <param name="isEnable"></param>
        /// <param name="isUsed"></param>
        /// <param name="isBack"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, string code,string account, bool? isEnable, bool? isUsed, bool? isBack,int size, int index, out int countSum);
        #endregion

        #region ѧԱ�Ŀ�
        /// <summary>
        /// ѧԱ����ѧϰ��������
        /// </summary>
        /// <param name="accid">ѧԱ�˺�id</param>
        /// <returns></returns>
        int AccountCardOfCount(int accid);
        /// <summary>
        /// ѧԱ����ѧϰ��������
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="state">״̬��0Ϊ��ʼ��1Ϊʹ�ã�-1Ϊ�ع�</param>
        /// <returns></returns>
        int AccountCardOfCount(int accid,int state);
        /// <summary>
        /// ѧԱ���µ�ѧϰ��
        /// </summary>
        /// <param name="accid">ѧԱ�˺�id</param>
        /// <param name="state">״̬��0Ϊ��ʼ��1Ϊʹ�ã�-1Ϊ�ع�</param>
        /// <returns></returns>
        LearningCard[] AccountCards(int accid, int state);
        /// <summary>
        /// ѧԱ���µ�����ѧϰ��
        /// </summary>
        /// <returns></returns>
        LearningCard[] AccountCards(int accid);
        /// <summary>
        /// ѧԱ���µ�ѧϰ������ҳ��ȡ
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="isused"></param>
        /// <param name="isback"></param>
        /// <param name="isdisable"></param>
        /// <param name="code"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="countSum">
        LearningCard[] AccountCards(int accid, bool? isused, bool? isback, bool? isdisable, string code, int index, int size, out int countSum);
        #endregion

        /// <summary>
        /// ����ѧԱ��ѧϰ�ɹ�
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        string ResultScoreToExcel(string filepath, int acid);
    }
}
