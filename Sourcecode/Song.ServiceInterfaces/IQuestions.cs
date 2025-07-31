using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using NPOI.HSSF.UserModel;
using WeiSha.Data;
using Newtonsoft.Json.Linq;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �������
    /// </summary>
    public interface IQuestions : WeiSha.Core.IBusinessInterface
    {
        
        #region �������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        long QuesAdd(Questions entity);       
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesSave(Questions entity);
        /// <summary>
        /// ������������ʱ�ô˷���
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        /// <param name="ansItem">��ʵ��</param>
        /// <returns></returns>
        void QuesInput(Questions entity, List<Song.Entities.QuesAnswer> ansItem);
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        void QuesDelete(Questions entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void QuesDelete(long identify);
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="ids"></param>
        void QuesDelete(string[] ids);
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="idarray"></param>
        void QuesDelete(long[] idarray);
        /// <summary>
        /// �޸������ĳЩ��
        /// </summary>
        /// <param name="qusid">����id</param>
        /// <param name="fields"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool QuesUpdate(long qusid, Field[] fields, object[] objs);
        /// <summary>
        /// �޸������ĳЩ��
        /// </summary>
        /// <param name="qusid"></param>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool QuesUpdate(long qusid, Field field, object obj);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Questions QuesSingle(long identify);
        /// <summary>
        ///  ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="cache">�Ƿ����Ի���</param>
        /// <returns></returns>
        Questions QuesSingle(long identify,bool cache);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�UID
        /// </summary>
        /// <param name="uid">ȫ��Ψһid</param>
        /// <returns></returns>
        Questions QuesSingle(string uid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰����
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type">���͵����ֱ�ʶ</param>
        /// <returns></returns>
        Questions QuesSingle(string title, int type);
        /// <summary>
        /// ��ǰ����Ĵ�
        /// </summary>
        /// <param name="qus">�������</param>
        /// <param name="isCorrect">�Ƿ�ȡ��ȷ�𰸣����ΪNullȡ���д𰸣����Ϊtrueȡ��ȷ��</param>
        /// <returns></returns>
        List<QuesAnswer> QuestionsAnswer(Questions qus, bool? isCorrect);
        /// <summary>
        /// ��ȡĳ���γ̻��½�����
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="isUse">�Ƿ�չʾ</param>
        /// <param name="count">ȡ��������С��1ȡ����</param>
        /// <returns></returns>
        List<Questions> QuesCount(int type, bool? isUse, int count);
        /// <summary>
        /// ��ȡĳ���γ̻��½�����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="index">��ʼ����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        List<Questions> QuesCount(int orgid, long  sbjid, long couid, long olid, int type, int diff, bool? isUse, int index, int count);
        /// <summary>
        /// ��ȡ�򻯵�ĳ���γ̻��½�����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="fields">Ҫȡֵ���ֶ�</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        List<Questions> QuesSimplify(int orgid, long sbjid, long couid, long olid, int type, int diff, bool? isUse, Field[] fields, int count);
        /// <summary>
        /// ͳ�����������������רҵ���½ڵ��¼�������,ֻȡ��ǰ�㼶
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse">�Ƿ���õ�</param>
        /// <returns></returns>
        int QuesOfCount(int orgid, long  sbjid, long couid, long olid, int type, int diff, bool? isUse);
        /// <summary>
        /// ͳ������������������רҵ���½ڵ��¼�������,ֻȡ��ǰ�㼶
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="types"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <returns></returns>
        int Total(int orgid, long sbjid, long couid, long olid, int[] types, int[] diff, bool? isUse, bool? isError, bool? isWrong);
        /// <summary>
        /// ͳ����������������¼�������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse">�Ƿ���õ�</param>
        /// <returns></returns>
        int Total(int orgid, long sbjid, long couid, long olid, int type, int diff, bool? isUse);
        /// <summary>
        /// �����������µ�������רҵ���γ̡��½ڣ�����չʾ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        void QuesCountUpdate(int orgid, long sbjid, long couid, long olid);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff1">�Ѷȷ�Χ</param>
        /// <param name="diff2">�Ѷȷ�Χ</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        List<Questions> QuesRandom(int orgid, long  sbjid, long couid, long olid, int type, int diff1, int diff2, bool? isUse, int count);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="sbjid">����ѧ�Ƶ�id</param>
        /// <param name="couid"></param>
        /// <param name="diff1">�Ѷȵȼ��������С�ȼ�</param>
        /// <param name="diff2">�Ѷȵȼ������ȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> QuesRandom(int type, long sbjid, long couid, int diff1, int diff2, bool? isUse, int count);
        /// <summary>
        /// ��ҳ��ȡ���е����⣻
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type">��������</param>
        /// <param name="isUse">�Ƿ���ʾ</param>
        /// <param name="diff">�����Ѷ�</param>
        /// <param name="searTxt">��ѯ�ַ�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Questions> QuesPager(int orgid, int type, bool? isUse, int diff, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���е����⣻
        /// </summary>
        /// <param name="orgid">��������id</param>
        /// <param name="type">��������</param>
        /// <param name="sbjid"></param>
        /// <param name="couid">�γ�Id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <param name="diff">�Ѷȵ�</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Questions> QuesPager(int orgid, int type, long sbjid, long couid, long olid, bool? isUse, bool? isError, bool? isWrong, int diff, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ�������һ�����⣬��ָ����Χ��ȡ������γ��ڵ�����
        /// </summary>
        /// <param name="id">����id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="sbjid">רҵid</param>
        /// <returns></returns>
        Questions QuesNext(long id, long olid, long couid, long sbjid);
        /// <summary>
        /// ���������һ�����⣬��ָ����Χ��ȡ������γ��ڵ�����
        /// </summary>
        /// <param name="id">����id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="sbjid">רҵid</param>
        /// <returns></returns>
        Questions QuesPrev(long id, long olid, long couid, long sbjid);

        #endregion

        #region ���⵼��
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="orgid">��������</param>
        /// <param name="type">�������ͣ��絥ѡ����ѡ��,��1,2�������ַ�������ʾ</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="diff">�Ѷȵȼ�����1,2�������ַ���</param>
        /// <param name="isError">�Ƿ������������⣬���Ϊ�գ������ж�</param>
        /// <param name="isWrong">�Ƿ����ѧԱ���������⣬���Ϊ�գ������ж�</param>
        /// <returns></returns>
        HSSFWorkbook QuestionsExport(string folder, int orgid, string type, long sbjid, long couid, long olid, string diff, bool? isError, bool? isWrong);
        /// <summary>
        /// ��������,�����ļ�
        /// </summary>
        /// <param name="subpath">�����ļ���·�����������ˣ��������ʱ·������·��</param>
        /// <param name="folder">�������ļ��У������subpath������һ��</param>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="diff"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>       
        /// <returns></returns>
        JObject QuestionsExportExcel(string subpath, string folder, int orgid, string type, long sbjid, long couid, long olid, string diff, bool? isError, bool? isWrong);
        #endregion

        #region ���͹���������ࣩ
        /// <summary>
        /// ���������б�
        /// </summary>
        /// <returns></returns>
        string[] QuestionTypes();
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int TypeAdd(QuesTypes entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void TypeSave(QuesTypes entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void TypeDelete(int identify);
        /// <summary>
        /// ����γ��µ��������
        /// </summary>
        /// <param name="couid">�γ�id</param>
        void TypeClear(long couid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        QuesTypes TypeSingle(int identify);
        /// <summary>
        /// ��ȡĳ��ѧ�Ƶ������������
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="isUse">�Ƿ�չʾ</param>
        /// <param name="count">ȡ��������С��1ȡ����</param>
        /// <returns></returns>
        QuesTypes[] TypeCount(long couid, bool? isUse, int count);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool TypeRemoveUp(int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool TypeRemoveDown(int id);
        #endregion

        #region ����𰸻�ѡ��
        /// <summary>
        /// ������Ĵ���ѡ��ת��Ϊxml�ַ���
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        string AnswerToItems(List<QuesAnswer> ans);
        /// <summary>
        /// ������ѡ���xml�ַ�����ת��ΪQuesAnswer��������
        /// </summary>
        /// <param name="qus"></param>
        /// <param name="isCorrect">�Ƿ񷵻���ȷ��ѡ�null����ȫ����trueֻ������ȷ�Ĵ𰸣�falseֻ���ش���</param>
        /// <returns></returns>
        List<QuesAnswer> ItemsToAnswer(Questions qus, bool? isCorrect);
        /// <summary>
        /// ������ѡ���xml�ַ�����ת��ΪQuesAnswer��������
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        Song.Entities.QuesAnswer ItemToAnswer(string xml);
        #endregion

        #region ����������
        /// <summary>
        /// ���������Ƿ�ش���ȷ
        /// </summary>
        /// <param name="qid">�����ID</param>
        /// <param name="ans">�𰸣�ѡ����Ϊid���ж���Ϊ���֣���ջ���Ϊ�ַ�</param>
        /// <returns>��ȷ����true</returns>
        bool IsAnseerCorrect(long qid, string ans);
        /// <summary>
        /// ���������Ƿ�ش���ȷ
        /// </summary>
        /// <param name="qus">�������</param>
        /// <param name="ans">�𰸣�ѡ����Ϊid���ж���Ϊ���֣���ջ���Ϊ�ַ�</param>
        /// <returns>��ȷ����true</returns>
        bool IsAnseerCorrect(Questions qus, string ans);
        /// <summary>
        /// ��������÷�
        /// </summary>
        /// <param name="qid">�����ID</param>
        /// <param name="ans">�𰸣�ѡ����Ϊid���ж���Ϊ���֣���ջ���Ϊ�ַ�</param>
        /// <param name="num">����ķ���</param>
        /// <returns>����ֵ�</returns>
        float CalcScore(long qid, string ans, float num);
        /// <summary>
        /// ��������÷�
        /// </summary>
        /// <param name="qus">�������</param>
        /// <param name="ans">�𰸣�ѡ����Ϊid���ж���Ϊ���֣���ջ���Ϊ�ַ�</param>
        /// <param name="num">����ķ���</param>
        /// <returns></returns>
        float CalcScore(Questions qus, string ans, float num);
        #endregion

        #region ������ϰ�ļ�¼
        /// <summary>
        /// ��¼������ϰ��¼
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="json"></param>
        /// <param name="sum">������</param>
        /// <param name="answer">������</param>
        /// <param name="correct">��ȷ��</param>
        /// <param name="wrong">������</param>
        /// <param name="rate">��ȷ��</param>
        /// <returns></returns>
        bool ExerciseLogSave(Accounts acc, int orgid, long couid, long olid, string json, int sum, int answer, int correct, int wrong,double rate);
        /// <summary>
        /// ��ȡ������ϰ��¼
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        LogForStudentExercise ExerciseLogGet(int acid, long couid, long olid);
        /// <summary>
        /// ɾ��������ϰ��¼
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        bool ExerciseLogDel(int acid, long couid, long olid);
        /// <summary>
        /// ����ĳ��ѧԱ����ϰ��¼��ͨ����
        /// </summary>
        /// <param name="acid">ѧԱ�˺�id</param>
        /// <param name="couid">�γ�id</param>
        /// <returns>����Ϊ�ٷֱ�</returns>
        double CalcPassRate(int acid, long couid);
        /// <summary>
        /// ĳ���γ̵�������ϰͨ����
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <returns>����Ϊ�ٷֱ�</returns>
        double CalcPassRate(long couid);

        #endregion

        #region ͳ����Ϣ
        /// <summary>
        /// ������Դ�Ĵ洢��С
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="count">��Դ����</param>
        /// <returns>��Դ�ļ��ܴ�С����λΪ�ֽ�</returns>
        long StorageResources(int orgid, long sbjid, long couid, long olid, out int count);
        #endregion
    }
}
