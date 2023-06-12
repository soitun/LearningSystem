using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ���Թ���
    /// </summary>
    public interface IExamination : WeiSha.Core.IBusinessInterface
    {

        #region ���Թ���
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int ExamAdd(Teacher teacher,Examination entity);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="theme">��������</param>
        /// <param name="items">���Եĳ���</param>
        /// <param name="groups">�ο���Ա�ķ�Χ</param>
        void ExamAdd(Teacher teacher, Examination theme, Examination[] items, ExamGroup[] groups);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ExamSave(Examination entity);
        /// <summary>
        /// �����޸�
        /// </summary>
        /// <param name="theme">��������</param>
        /// <param name="items">���Եĳ���</param>
        /// <param name="groups">�ο���Ա�ķ�Χ</param>
        void ExamSave(Examination theme, Examination[] items, ExamGroup[] groups);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void ExamDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID���˴���ȡ���ǿ�������򳡴�
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Examination ExamSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ�����ͨ��ȫ��Ψһֵ���˴���ȡ���ǿ�������
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Examination ExamSingle(string uid);
        /// <summary>
        /// ��ȡ��һʵ�����ȡ���һ�ο��ԣ��˴���ȡ���ǿ�������򳡴�
        /// </summary>
        /// <returns></returns>
        Examination ExamLast();
        /// <summary>
        /// ��ȡ��ǰ���ԵĿ�����Ŀ
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Examination[] ExamItem(string uid);
        Examination[] ExamItem(int id);
        /// <summary>
        /// ��ǰ�������������ѧ������
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        StudentSort[] GroupForStudentSort(string uid);  
        /// <summary>
        /// ��ȡ���ԣ�����ҳ
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Examination> ExamCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// ��ȡ��ǰѧ��Ҫ�μӵĿ���
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="start">ʱ�䷶Χ��ѯ�Ŀ�ʼʱ��</param>
        /// <param name="end">ʱ�䷶Χ��ѯ�Ľ���ʱ��</param>
        /// <param name="search">�����������</param>
        /// <returns></returns>
        List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end, string search);
        List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end, string search, int size, int index, out int countSum);
        /// <summary>
        /// �ж�ĳ�������Ƿ�����ĳ��ѧ���μ�
        /// </summary>
        /// <param name="examid">����id</param>
        /// <param name="stid">ѧ��id</param>
        /// <returns></returns>
        bool ExamIsForStudent(int examid, int stid);
        /// <summary>
        /// ��ȡָ��ʱ�����ݵĿ���
        /// </summary>
        /// <param name="start">ʱ����������Ŀ�ʼʱ��</param>
        /// <param name="end">ʱ�����������ĩβʱ��</param>
        /// <param name="isUse"></param>
        /// <param name="searName"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Examination[] GetPager(int orgid, DateTime? start, DateTime? end, bool? isUse, string searName, int size, int index, out int countSum);
        /// <summary>
        /// ��ȡ��ǰѧ���μӵĵĿ���
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="orgid"></param>
        /// <param name="sear"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ExamResults[] GetAttendPager(int stid, long  sbjid, int orgid, string sear, int size, int index, out int countSum);
        #endregion

        #region ���Գɼ��ύ��
        /// <summary>
        /// ��ӿ��Դ�����Ϣ
        /// </summary>
        /// <param name="result"></param>
        ExamResults ResultAdd(ExamResults result);
        /// <summary>
        /// ���濼�Դ�����Ϣ
        /// </summary>
        /// <param name="result"></param>
        void ResultSave(ExamResults result);
        /// <summary>
        /// �ɼ��ύ
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        ExamResults ResultSubmit(ExamResults result);
        /// <summary>
        /// ����ɼ�������
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Song.Entities.ExamResults ClacScore(ExamResults result);
        /// <summary>
        /// ɾ�����Գɼ�
        /// </summary>
        /// <param name="id">�ɼ���¼��id</param>
        void ResultDelete(int id);
        /// <summary>
        /// ɾ��ĳ��ѧ����ĳ�����Եĳɼ�
        /// </summary>
        /// <param name="stid">ѧԱ�˺�id</param>
        /// <param name="examid">����id</param>
        void ResultDelete(int stid, int examid);
        /// <summary>
        /// ɾ�������µ����гɼ�
        /// </summary>
        /// <param name="examid">����id</param>
        void ResultClear(int examid);
        /// <summary>
        /// ��ȡ���µĴ�����Ϣ����ʽ������Ϣ��
        /// </summary>
        /// <param name="examid">����id</param>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="acid">����id</param>
        /// <returns></returns>
        ExamResults ResultSingle(int examid, long tpid, int acid);
        /// <summary>
        /// �ӻ����л�ȡ���Դ�����Ϣ
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        ExamResults ResultForCache(int examid, long tpid, int acid);
        /// <summary>
        /// ���´�����Ϣ����
        /// </summary>
        /// <param name="exr"></param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        string ResultCacheUpdate(ExamResults exr, int expires, int examid, long tpid, int acid);
        /// <summary>
        /// ѧԱ��ĳ�����Գ��εĵ÷�
        /// </summary>
        /// <param name="examid">���Գ���id</param>
        /// <param name="acid">ѧԱid</param>
        /// <returns></returns>
        double? ResultScore(int acid, int examid);      
        /// <summary>
        /// ��ȡ��ǰ���Ե����п���������Ϣ
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ExamResults[] ResultCount(int examid, int count);
        /// <summary>
        /// ��ǰ������Ϣ�У���һ��
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="stid"></param>
        /// <param name="isCorrect">�Ƿ����˹��о���ģ�false��һ��δ�о����Ϣ</param>
        /// <returns></returns>
        ExamResults ResultSingleNext(int examid, int stid, bool? isCorrect);
        /// <summary>
        /// ͨ����id��ȡ������Ϣ����ʽ������Ϣ��
        /// </summary>
        /// <param name="exrid"></param>
        /// <returns></returns>
        ExamResults ResultSingle(int exrid);
        /// <summary>
        /// ͨ��ѧԱID�뿼��ID����ȡ�ɼ�����óɼ���
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="examid"></param>
        /// <returns></returns>
        ExamResults ResultSingle(int accid, int examid);
        /// <summary>
        /// ���㵱ǰ���Խ���ĳɼ�
        /// </summary>
        /// <param name="resu"></param>
        /// <returns></returns>
        ExamResults ResultClacScore(ExamResults resu);
        /// <summary>
        /// ���ݴ�����Ϣ����ȡ���⣨��Դ��������������������ʱ��
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        List<Questions> QuesForResults(string results);
        #endregion

        #region �ɼ�ͳ��
        /// <summary>
        /// ���������µ����вο���Ա�ɼ�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable Result4Theme(int id);
        /// <summary>
        /// ���������µ����вο���Ա�İ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentSort[] StudentSort4Theme(int id);
        /// <summary>
        /// ���Գ����µ�ѧԱ��
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        StudentSort[] StudentSort4Exam(int examid);
        /// <summary>
        /// ���������µ����вο���Ա�ɼ�
        /// </summary>
        /// <param name="id">��ǰ���������ID</param>
        /// <param name="stsid">ѧ�������id��Ϊ0ʱȡ���У�Ϊ-1ʱȡ�������ѧԱ������0��ȡ��ǰ��ѧԱ</param>
        /// <returns></returns>
        DataTable Result4Theme(int examid, long stsid);
        /// <summary>
        /// ���������µ����вο���Ա�ɼ�
        /// </summary>
        /// <param name="id">��ǰ���������ID</param>
        /// <param name="stsid">ѧ�������id��Ϊ0ʱȡ���У�Ϊ-1ʱȡ�������ѧԱ������0��ȡ��ǰ��ѧԱ</param>
        /// <returns></returns>
        DataTable Result4Theme(int examid, string stsid);
        /// <summary>
        /// ���������µ����вο���Ա�ɼ�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stsid">ѧ�������id��Ϊ0ʱȡ���У�Ϊ-1ʱȡ�������ѧԱ������0��ȡ��ǰ��ѧԱ</param>
        /// <param name="isAll">�Ƿ�ȡ������Ա����ȱ����Ա��,falseΪ���ο���Ա</param>
        /// <returns></returns>
        DataTable Result4Theme(int id, long stsid, bool isAll);
        /// <summary>
        /// ��ǰ���������µĸ�ѧԱ����ɼ�����
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        DataTable Result4StudentSort(int examid);        
        /// <summary>
        /// ����ĳ����������ļ�����
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        double PassRate4Theme(string uid);
        /// <summary>
        /// ����ĳ�����Եļ�����
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        double PassRate4Exam(Examination exam);
        /// <summary>
        /// ����ĳ�����Եļ�����
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        double PassRate4Exam(int examid);
        /// <summary>
        /// ����ĳ�����������ƽ����
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        double Avg4Theme(string uid);
        /// <summary>
        /// ����ĳ�����Ե�ƽ����
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        double Avg4Exam(int examid);
        /// <summary>
        /// ĳ�����Ե���߷�
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        double Highest4Exam(int examid);
        /// <summary>
        /// ĳ�����Ե���ͷ�
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        double Lowest4Exam(int examid);
        /// <summary>
        /// ��ǰ���ԵĲο�����
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        int Number4Exam(int examid);
        /// <summary>
        /// �μӿ��������ѧԱ�б�
        /// </summary>
        /// <param name="id">���������id</param>
        /// <param name="name"></param>
        /// <param name="idcard"></param>
        /// <param name="stsid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Accounts> AttendThemeAccounts(int id, string name, string idcard, int stsid, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ���Գ����µ�������Ա�ɼ�
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="name">ѧԱ����</param>
        /// <param name="idcard">���֤��</param>
        /// <param name="stsid"></param>
        /// <param name="min">�����������ȡ��¼���˴�����ͷ�</param>
        /// <param name="max">��߷�</param>
        /// <param name="manual">�Ƿ�����</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ExamResults[] Results(int examid, string name, string idcard, int stsid, float min, float max,bool? manual, int size, int index, out int countSum);
        ExamResults[] Results(string examuid, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ���Գ����µ�������Ա�ɼ�
        /// </summary>
        /// <param name="examid">���Գ���id</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        ExamResults[] Results(int examid, int count);
        #endregion

        #region �ɼ�����
        /// <summary>
        /// ĳ�����Եĳɼ�����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="examid">���Գ���id</param>
        /// <returns></returns>
        string Export4Excel(string filePath, int examid);
        /// <summary>
        /// �����μӿ��Ե�ѧԱ�ɼ�
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="examid">���������id</param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        string OutputParticipate(string filePath, int examid, StudentSort[] sorts);

        string OutputAll(string filePath, int examid);
        /// <summary>
        /// ѧԱ��ĳ���γ��µĿ��Գɼ�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acid"></param>
        /// <returns>���سɼ���¼</returns>
        ExamResults StudentForCourseExam(long couid, int acid);
        #endregion

    }
}
