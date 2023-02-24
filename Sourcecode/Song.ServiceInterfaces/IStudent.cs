using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ѧԱ�Ĺ���
    /// </summary>
    public interface IStudent : WeiSha.Core.IBusinessInterface
    {
        #region ѧԱ���ࣨ�飩����
        /// <summary>
        /// ���ѧԱ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SortAdd(StudentSort entity);
        /// <summary>
        /// �޸�ѧԱ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SortSave(StudentSort entity);
        /// <summary>
        /// �޸�ѧԱ���״̬
        /// </summary>
        /// <param name="stsid">ѧԱ��id</param>
        /// <param name="use">�Ƿ�����</param>
        /// <returns></returns>
        bool SortUpdateUse(long stsid, bool use);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns>���ɾ���ɹ�������0����������ѧԱ������-1�������Ĭ���飬����-2</returns>
        int SortDelete(long identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        StudentSort SortSingle(long identify);
        /// <summary>
        /// ����ѧԱ�����ƻ�ȡѧԱ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        StudentSort SortSingle(string name,int orgid);
        /// <summary>
        /// ��ȡĬ��ѧԱ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        StudentSort SortDefault(int orgid);
        /// <summary>
        /// ����Ĭ��ѧԱ����
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify"></param>
        /// <returns></returns>
        void SortSetDefault(int orgid, long identify);
        /// <summary>
        /// ��ȡ���󣻼�����ѧԱ�飻
        /// </summary>
        /// <returns></returns>
        StudentSort[] SortAll(int orgid, bool? isUse);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StudentSort> SortCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// ��ȡĳ��վѧԱ�������飻
        /// </summary>
        /// <param name="studentId">ѧԱid</param>
        /// <returns></returns>
        StudentSort Sort4Student(int studentId);
        /// <summary>
        /// ��ȡĳ�����������վѧԱ
        /// </summary>
        /// <param name="stsid">����id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Accounts[] Student4Sort(long stsid, bool? isUse);
        /// <summary>
        /// ѧԱ���е�ѧԱ����
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        int SortOfNumber(long stsid);
        /// <summary>
        /// ����ѧԱ���е�ѧԱ����
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        void SortUpdateNumber(long stsid);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <returns></returns>
        bool SortIsExist(StudentSort entity);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="name">ѧԱ������</param>
        /// <param name="id">ѧԱ��id</param>
        /// <param name="orgid">���ڻ���id</param>
        /// <returns></returns>
        bool SortIsExist(string name, long id, int orgid);
        /// <summary>
        /// ����ѧԱ�������
        /// </summary>
        /// <param name="items">ѧԱ���ʵ������</param>
        /// <returns></returns>
        bool SortUpdateTaxis(Song.Entities.StudentSort[] items);
        /// <summary>
        /// ��ҳ��ȡѧԱ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isUse"></param>
        /// <param name="name">��������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        StudentSort[] SortPager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
        #endregion

        #region ѧԱ����γ�
        /// <summary>
        /// ����ѧԱ����γ̵Ĺ���
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        int SortCourseAdd(long stsid, long couid);
        /// <summary>
        /// ����ѧԱ����γ̵Ĺ���
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns></returns>
        int SortCourseAdd(StudentSort_Course ssc);
        /// <summary>
        /// �޸�ѧԱ����γ̵Ĺ���
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns></returns>
        int SortCourseSave(StudentSort_Course ssc);
        /// <summary>
        /// ѧԱ������Ŀγ���
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        int SortCourseCount(long stsid);
        /// <summary>
        /// ɾ��ѧԱ����γ̵Ĺ���
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool SortCourseDelete(long stsid, long couid);
        /// <summary>
        /// �ж�ĳ���γ��Ƿ������ѧԱ��
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stsid">ѧԱ��id</param>
        /// <returns></returns>
        bool SortExistCourse(long couid, long stsid);
        /// <summary>
        /// ��ѧԱ������Ŀγ̣�������Student_Course��ѧԱ��γ̵Ĺ�����
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="acid">ѧԱ�˺�id</param>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        Student_Course SortCourseToStudent(int acid, long couid);
        /// <summary>
        /// ��ѧԱ������Ŀγ̣�������Student_Course��ѧԱ��γ̵Ĺ�����
        /// </summary>
        /// <param name="acc">ѧԱ�˺�</param>
        /// <param name="couid">�γ�id</param>
        /// <returns></returns>
        Student_Course SortCourseToStudent(Accounts acc, long couid);
        /// <summary>
        /// ѧԱ����������пγ�
        /// </summary>
        /// <param name="stsid">ѧԱ���id</param>
        /// <param name="name">�����Ƽ���</param>
        /// <returns></returns>
        List<Course> SortCourseList(long stsid, string name);
        /// <summary>
        /// ��ҳ��ȡѧԱ������Ŀγ�
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> SortCoursePager(long stsid, string name, int size, int index, out int countSum);
        #endregion

        #region ѧԱ��¼�����߼�¼
        /// <summary>
        /// ��ӵ�¼��¼
        /// </summary>
        /// <returns></returns>
        void LogForLoginAdd(Accounts st);
        /// <summary>
        /// �޸ĵ�¼�ǣ�ˢ��һ�µ�¼��Ϣ����������ʱ��
        /// </summary>
        /// <param name="interval">�����ύ�ļ��ʱ�䣬Ҳ��ÿ���ύ�����ӵ�����ʱ��������λ��</param>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        void LogForLoginFresh(Accounts st, int interval, string plat);
        /// <summary>
        /// �˳���¼֮ǰ�ļ�¼����
        /// </summary>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        void LogForLoginOut(Accounts st, string plat);
        /// <summary>
        /// ����ѧԱid���¼ʱ���ɵ�Uid����ʵ��
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="stuid">��¼ʱ���ɵ�����ַ�����ȫ��Ψһ</param>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int stid, string stuid, string plat);
        /// <summary>
        /// ���ؼ�¼
        /// </summary>
        /// <param name="identify">��¼ID</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int identify);
        /// <summary>
        /// ɾ��ѧԱ���߼�¼
        /// </summary>
        /// <param name="identify"></param>
        void StudentOnlineDelete(int identify);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="stid">ѧԱId</param>
        /// <param name="platform">ѧԱ����ƽ̨��PC��Mobi</param>
        /// <param name="start">ͳ�ƵĿ�ʼʱ��</param>
        /// <param name="end">ͳ�ƵĽ���ʱ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="stid"></param>
        /// <param name="platform"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stname">ѧԱ����</param>
        /// <param name="stmobi">ѧԱ�ֻ���</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, string stname, string stmobi, int size, int index, out int countSum);
        #endregion

        #region ѧԱ��Ƶѧϰ�ļ�¼        
        /// <summary>
        /// ��¼ѧԱѧϰʱ��
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">�½�id</param>
        /// <param name="st">ѧԱ�˻�</param>
        /// <param name="playTime">���Ž���</param>
        /// <param name="studyInterval">ѧϰʱ�䣬��Ϊʱ������ÿ���ύѧϰʱ��������</param>
        /// <param name="totalTime">��Ƶ�ܳ���</param>
        void LogForStudyFresh(long couid, long olid, Accounts st, int playTime, int studyInterval, int totalTime);
        /// <summary>
        /// ��¼ѧԱѧϰʱ��
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">�½�id</param>
        /// <param name="st">ѧԱ�˻�</param>
        /// <param name="playTime">���Ž���</param>
        /// <param name="studyTime">ѧϰʱ�䣬��Ϊ�ۼ�ʱ��</param>
        /// <param name="totalTime">��Ƶ�ܳ���</param>
        /// <returns>ѧϰ���Ȱٷֱȣ��������ʱ���������Ϊ-1�����ʾʧ��</returns>
        void LogForStudyUpdate(long couid, long olid, Accounts st, int playTime, int studyTime, int totalTime);
        /// <summary>
        /// ����ѧԱid���½�id
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="olid">�½�id</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int stid, long olid);
        /// <summary>
        /// ���ؼ�¼
        /// </summary>
        /// <param name="identify">��¼ID</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int identify);
        /// <summary>
        /// ����ѧϰ��¼
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="platform">ƽ̨��PC��Mobi</param>
        /// <param name="count"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyCount(int orgid, long couid, long olid, int stid, string platform, int count);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid">ѧԱId</param>
        /// <param name="platform">ѧԱ����ƽ̨��PC��Mobi</param>    
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyPager(int orgid, long couid, long olid, int stid, string platform, int size, int index, out int countSum);
        /// <summary>
        /// ѧԱ����ѧϰ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid"></param>
        /// <returns>datatable��LastTime��Ϊѧϰʱ�䣻studyTime��ѧϰʱ��</returns>
        DataTable StudentStudyCourseLog(int stid);
        /// <summary>
        /// ѧԱָ��ѧϰ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couids">�γ�id,���ŷָ�</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid, string couids);
        /// <summary>
        /// ѧԱ����ѧϰĳһ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="couid">�γ�id</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid, long couid);
        /// <summary>
        /// ѧԱѧϰĳһ�γ��������½ڵļ�¼
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱ�˻�id</param>
        /// <returns>datatable�У�LastTime�����ѧϰʱ�䣻totalTime����Ƶʱ�䳤��playTime�����Ž��ȣ�studyTime��ѧϰʱ�䣬complete����ɶȰٷֱ�</returns>
        DataTable StudentStudyOutlineLog(long couid, int stid);
        /// <summary>
        /// �½�ѧϰ��¼���ף�ֱ�ӽ�ѧϰ��������Ϊ100
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        void CheatOutlineLog(int stid, long olid);
        #endregion

        #region ѧԱ�Ĵ���ع�
        /// <summary>
        /// ������ѧԱ�Ĵ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesAdd(Student_Ques entity);
        /// <summary>
        /// �޸�ѧԱ�Ĵ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesSave(Student_Ques entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void QuesDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid">����id</param>
        /// <param name="stid">ѧԱid</param>
        void QuesDelete(long quesid, int stid);
        /// <summary>
        /// ��մ���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns>���������</returns>
        int QuesClear(long couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Ques QuesSingle(int identify);
        /// <summary>
        /// ��ǰѧԱ�����д���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid"></param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] QuesAll(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid"></param>
        /// <param name="type">��������</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesCount(int stid, long sbjid, long couid, int type, int count);
        /// <summary>
        /// ѧԱ����ĸ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">רҵ id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        int QuesOfCount(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// ��Ƶ����
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <param name="type">����</param>
        /// <param name="count">ȡ������</param>
        /// <returns>����������ṹ+count�У�ȡ����Ĵ������</returns>
        Questions[] QuesOftenwrong(long couid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid"></param>
        /// <param name="type">��������</param>
        /// <param name="diff">���׶�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int stid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum);
        /// <summary>
        /// ���������Ŀγ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="couname">�γ����ƣ���ģ����ѯ</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Course[] QuesForCourse(int stid, string couname, int size, int index, out int countSum);
        #endregion

        #region ѧԱ�������ղ�
        /// <summary>
        /// ������ѧԱ�ղص�����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CollectAdd(Student_Collect entity);
        /// <summary>
        /// �޸�ѧԱ�ղص�����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CollectSave(Student_Collect entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void CollectDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void CollectDelete(long quesid, int stid);
        /// <summary>
        /// ��������ղ�
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        void CollectClear(long couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Collect CollectSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ�壬ͨ��ѧԱ������id
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        Student_Collect CollectSingle(int acid, long qid);
        /// <summary>
        /// ��ǰѧԱ�ղص�����
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] CollectAll4Ques(int stid, long sbjid, long couid, int type);
        Student_Collect[] CollectAll(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] CollectCount(int stid, long sbjid, long couid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">���׶�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] CollectPager(int stid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum);
        #endregion

        #region ѧԱ�ıʼ�
        /// <summary>
        /// ������ѧԱ�ıʼ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NotesAdd(Student_Notes entity);
        /// <summary>
        /// �޸�ѧԱ�ıʼ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NotesSave(Student_Notes entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void NotesDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void NotesDelete(long quesid, int stid);
        /// <summary>
        /// �������ʼ�
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        void NotesClear(long couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Notes NotesSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����id��ѧԱid
        /// </summary>
        /// <param name="quesid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns></returns>
        Student_Notes NotesSingle(long quesid, int stid);
        /// <summary>
        /// ��ǰѧԱ�����бʼ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Student_Notes[] NotesAll(int stid, int type);
        /// <summary>
        /// ȡ��ǰѧԱ�ıʼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, long couid, int type, int count);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">��������</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="quesid">����id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Student_Notes[] NotesPager(int stid, long quesid, string searTxt, int size, int index, out int countSum);
        #endregion
    }
}
