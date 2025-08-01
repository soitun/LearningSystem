using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;
using System.Xml;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �Ծ�Ĺ���
    /// </summary>
    public interface ITestPaper : WeiSha.Core.IBusinessInterface
    {
        #region �Ծ����
        /// <summary>
        /// ����Ծ�
        /// </summary>
        /// <param name="entity">�Ծ����</param>
        long PaperAdd(TestPaper entity);
        /// <summary>
        /// �޸��Ծ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PaperSave(TestPaper entity);
        /// <summary>
        /// �޸��Ծ��ĳЩ��
        /// </summary>
        /// <param name="id">�Ծ��id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool PaperUpdate(long id, Field[] fiels, object[] objs);
        /// <summary>
        /// ɾ���Ծ�������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void PaperDelete(long identify);
        /// <summary>
        /// ��ȡ��һ�Ծ�ʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        TestPaper PaperSingle(long identify);
        /// <summary>
        /// ��ȡ��һ�Ծ�ʵ����󣬰��Ծ����ƣ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TestPaper PaperSingle(string name);
        /// <summary>
        /// ��ȡĳ���γ̵Ľ�ο���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="use"></param>
        /// <returns></returns>
        TestPaper FinalPaper(long couid, bool? use);
        /// <summary>
        /// ��ȡָ�����ݵ��Ծ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">ָ������</param>
        /// <returns></returns>
        TestPaper[] PaperCount(int orgid, long sbjid, long couid, int diff, bool? isUse, int count);
        /// <summary>
        /// ��ȡָ�����ݵ��Ծ�
        /// </summary>
        /// <param name="search"></param>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">ָ������</param>
        /// <returns></returns>
        TestPaper[] PaperCount(string search, int orgid, long sbjid, long couid, int diff, bool? isUse, int count);
        /// <summary>
        /// �����ж��ٸ��Ծ�
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int PaperOfCount(int orgid, long sbjid, long couid, int diff, bool? isUse);
        /// <summary>
        /// �Ծ��������µ�רҵ���γ̣�����չʾ
        /// </summary>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        void PaperCountUpdate(long sbjid, long couid);
        /// <summary>
        /// ��ҳ��ȡ�Ծ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid"></param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse">�Ƿ�ʹ��</param>
        /// <param name="sear">�������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestPaper[] PaperPager(int orgid, long sbjid, long couid, int diff, bool? isUse, string sear, int size, int index, out int countSum);

        #endregion

        #region �Ծ��������
        /// <summary>
        /// ���γ̳���ʱ���Ծ������ռ�������
        /// </summary>
        /// <param name="tp">�Ծ����</param>
        /// <returns></returns>
        List<TestPaperItem> GetItemForAll(TestPaper tp);
        /// <summary>
        /// ���½ڳ���ʱ��������ռ��
        /// </summary>
        /// <param name="tp">�Ծ����</param>
        /// <returns></returns>
        List<TestPaperItem> GetItemForOlPercent(TestPaper tp);
        /// <summary>
        /// ���½ڳ���ʱ�����½���������
        /// </summary>
        /// <param name="tp">�Ծ����</param>
        /// <param name="olid">�½�id�����С��1����ȡ����</param>
        /// <returns></returns>
        List<TestPaperItem> GetItemForOlCount(TestPaper tp, long olid);
        /// <summary>
        /// �����Ծ�Ĵ�������ǰ��γ̣����ǰ��½�
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        List<TestPaperItem> GetItemForAny(TestPaper tp);
        #endregion

        #region ����
        /// <summary>
        /// ��������Ծ�����
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="isanswer">�����Ƿ���𰸣�ģ�⿼��һ����𰸣�����ǰ�˼���ɼ�</param>
        /// <returns></returns>
        Dictionary<TestPaperItem, List<Questions>> Putout(long tpid, bool isanswer);
        /// <summary>
        /// ��������Ծ�����
        /// </summary>
        /// <param name="tp">�Ծ����</param>
        /// <param name="isanswer">�����Ƿ���𰸣�ģ�⿼��һ����𰸣�����ǰ�˼���ɼ�</param>
        /// <returns></returns>
        Dictionary<TestPaperItem, List<Questions>> Putout(TestPaper tp, bool isanswer);
        /// <summary>
        /// ��������ʷ�������������Ծ�
        /// </summary>
        /// <param name="results">ѧԱ�����xml��¼</param>
        /// <param name="isanswer">�����Ƿ���𰸣�ģ�⿼��һ����𰸣�����ǰ�˼���ɼ�</param>
        /// <returns></returns>
        Dictionary<TestPaperItem, List<Questions>> Putout(string results, bool isanswer);
        /// <summary>
        /// ��������ʷ�������������Ծ�
        /// </summary>
        Dictionary<TestPaperItem, List<Questions>> Putout(XmlDocument resxml, bool isanswer);
        #endregion

        #region �Ծ���ԵĴ���
        /// <summary>
        /// ��Ӳ��Գɼ�,���ص÷�
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="force">ǿ�Ƽ��㣬Ĭ�����ڿͻ��˼�������ߵģ�����ǿ���ټ���</param>
        /// <returns>���ص÷�</returns>
        float ResultsAdd(TestResults entity,bool force);
        /// <summary>
        /// �޸Ĳ��Գɼ�,���ص÷�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <param name="force">ǿ�Ƽ��㣬Ĭ�����ڿͻ��˼�������ߵģ�����ǿ���ټ���</param>
        /// <returns>���ص÷�</returns>
        float ResultsSave(TestResults entity, bool force);
        /// <summary>
        /// ����ɼ������ݳɼ�id
        /// </summary>
        /// <param name="trid"></param>
        /// <returns></returns>
        float ResultsCalc(int trid);
        /// <summary>
        /// ���������Ծ�����гɼ�
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        /// <returns></returns>
        bool ResultsBatchCalc(long tpid);
        /// <summary>
        /// ��ǰ���Եļ�����
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsPassrate(long identify);
        /// <summary>
        /// �ο��˴�
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        int ResultsPersontime(long identify);
        /// <summary>
        /// ������Ծ�����в��Ե�ƽ����
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsAverage(long identify);
        /// <summary>
        /// ������Ծ�����в��Ե���߷�
        /// </summary>
        /// <param name="identify">�Ծ�id</param>
        /// <returns></returns>
        TestResults ResultsHighest(long identify);
        /// <summary>
        /// ������Ծ��ĳ��ѧԱ����߷�
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns></returns>
        float ResultsHighest(long tpid,int stid);
        /// <summary>
        /// ������Ծ�����в��Ե���ͷ�
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        TestResults ResultsLowest(long identify);
        /// <summary>
        /// ɾ�����Գɼ���������ID��
        /// </summary>
        /// <param name="identify">�ɼ�id</param>
        void ResultsDelete(int identify);
        /// <summary>
        /// ���ĳ���Ծ��ĳ��ѧԱ�����в��Գɼ�
        /// </summary>
        /// <param name="acid">ѧԱid</param>
        /// <param name="tpid">�Ծ�id</param>
        int ResultsClear(int acid, long tpid);
        /// <summary>
        /// ���ĳ���Ծ�����в��Գɼ�
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        int ResultsClear(long tpid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        TestResults ResultsSingle(int identify);
        /// <summary>
        /// ��ȡĳԱ���Ĳ��Գɼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TestResults[] ResultsCount(int stid, long couid, string search, int count);
        /// <summary>
        /// ��ȡĳԱ���Ĳ��Գɼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="tpid"></param>    
        /// <returns></returns>
        TestResults[] ResultsCount(int stid, long tpid);
        /// <summary>
        /// �Ծ�ĳɼ��������μӿ��Ե��˴�
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        /// <returns></returns>
        int ResultsOfCount(long tpid);
        /// <summary>
        /// ��ҳ��ȡ���Գɼ�
        /// </summary>
        /// <param name="stid">ѧԱ�˺�id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, long sbjid, long couid, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���Գɼ�
        /// </summary>
        /// <param name="stid">ѧԱ�˺�id</param>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="tpname">�Ծ�����</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="orgid">����id</param>
        /// <param name="acc">ѧԱ�˺�</param>
        /// <param name="cardid">���֤��</param>
        /// <param name="score_min">�ɼ���ѯ��Χ����Сֵ</param>
        /// <param name="score_max">�ɼ���ѯ��Χ�����ֵ</param>
        /// <param name="time_min">����ʱ��Ĳ�ѯ��Χ����Сֵ</param>
        /// <param name="time_max">����ʱ��Ĳ�ѯ��Χ�����ֵ</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, long tpid, string tpname, long couid, long sbjid, int orgid,
            string acc, string cardid, float score_min, float score_max, DateTime? time_min, DateTime? time_max,
            int size, int index, out int countSum);
        /// <summary>
        /// ���Ծ��ҳ���ز��Գɼ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, long tpid, int size, int index, out int countSum);

        /// <summary>
        /// �ɼ�����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tpid">�Ծ�id</param>
        /// <returns></returns>
        string ResultsOutput(string filePath, long tpid);
        #endregion
    }
}
