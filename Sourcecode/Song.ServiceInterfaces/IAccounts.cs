using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;
using NPOI.HSSF.UserModel;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �˻�����
    /// </summary>
    public interface IAccounts : WeiSha.Core.IBusinessInterface
    {
        #region �˻�����
        /// <summary>
        /// ע��Э��
        /// </summary>
        /// <returns></returns>
        string RegAgreement();
        /// <summary>
        /// ����˻�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <returns>����Ѿ����ڸ��˻����򷵻�-1</returns>
        int AccountsAdd(Accounts entity);
        /// <summary>
        /// �޸��˻�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void AccountsSave(Accounts entity);
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pw">���룬δ���ܵ�����</param>
        void AccountsUpdatePw(Accounts entity,string pw);
        /// <summary>
        /// �޸��˻����������޸�
        /// </summary>
        /// <param name="entity">�˺Ŷ����ʵ��</param>
        /// <param name="fiels">Ҫ�޸ĵ��ֶ�</param>
        /// <param name="objs">fiels��Ӧ��ֵ</param>
        void AccountsUpdate(Accounts entity, Field[] fiels, object[] objs);
        /// <summary>
        /// �޸��˻����������޸�
        /// </summary>
        /// <param name="acid">�˺�ID</param>
        /// <param name="fiels">Ҫ�޸ĵ��ֶ�</param>
        /// <param name="objs">fiels��Ӧ��ֵ</param>
        void AccountsUpdate(int acid, Field[] fiels, object[] objs);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void AccountsDelete(int identify);
        /// <summary>
        /// ɾ���˻�
        /// </summary>
        /// <param name="entity">�˺Ŷ����ʵ��</param>
        void AccountsDelete(Accounts entity);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Accounts AccountsSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰��˻�����
        /// </summary>
        /// <param name="accname">�˻�����</param>
        /// <param name="pw">����</param>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        Accounts AccountsSingle(string accname, string pw, int orgid);
        /// <summary>
        /// ͨ���˺�����ȡ
        /// </summary>
        /// <param name="accname">�˻�����</param>
        /// <param name="orgid">����ID</param>
        /// <returns></returns>
        Accounts AccountsSingle(string accname, int orgid);
        /// <summary>
        /// ͨ���ֻ��Ż�ȡ�˻�
        /// </summary>
        /// <param name="phone">�ֻ���</param>
        /// <param name="orgid">����ID</param>
        /// <param name="isPass">�Ƿ�ͨ�����</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <returns></returns>
        Accounts AccountsForMobi(string phone, int orgid, bool? isPass, bool? isUse);
        /// <summary>
        /// �����֤�Ų�ѯ�˺�
        /// </summary>
        /// <param name="idcard">����ú�</param>
        /// <param name="orgid">����ID</param>
        /// <param name="isPass">�Ƿ����ͨ��</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <returns></returns>
        Accounts AccountsForIDCard(string idcard, int orgid, bool? isPass, bool? isUse);
        /// <summary>
        /// ��ȡ��һʵ�壬ͨ��id����֤��
        /// </summary>
        /// <param name="id">�˻�Id</param>
        /// <param name="uid">�˻���¼ʱ��������ַ��������ж�ͬһ�˺Ų�ͬ�˵�¼������</param>
        /// <returns></returns>
        Accounts AccountsSingle(int id, string uid);
        /// <summary>
        /// ͨ��������ȡ�˺�
        /// </summary>
        /// <param name="name">�˺�����</param>
        /// <returns></returns>
        Accounts[] Account4Name(string name);
        /// <summary>
        /// ��ѯ��������¼�˺��Ƿ����
        /// </summary>
        /// <param name="openid">��������¼��id</param>
        /// <param name="field">�ڱ�ϵͳaccounts����ֶ�</param>
        /// <returns></returns>
        Accounts AccountThirdparty(string openid,string field);
        /// <summary>
        /// ͨ�������˺ŵ�id����ȡ��ʦ�˻�
        /// </summary>
        /// <param name="acid">�˺�ID</param>
        /// <param name="isPass">�Ƿ�ͨ����˵ģ�nullΪ����</param>
        /// <returns></returns>
        Teacher GetTeacher(int acid, bool? isPass);
        /// <summary>
        /// ��¼��֤
        /// </summary>
        /// <param name="acc">�˺ţ������֤�����ֻ�</param>
        /// <param name="pw">���루���ģ�δ��md5���ܣ�</param>
        /// <param name="isPass">�Ƿ����ͨ��</param>
        /// <returns></returns>
        Accounts AccountsLogin(string acc, string pw, bool? isPass);
        /// <summary>
        /// ��¼�жϣ���������
        /// </summary>
        /// <param name="acc">�˺�</param>
        /// <param name="isPass">�Ƿ����ͨ��</param>
        /// <returns></returns>
        Accounts AccountsLogin(string acc, bool? isPass);
        /// <summary>
        /// ֱ�ӵ�¼
        /// </summary>
        /// <param name="acc">�˺Ŷ���</param>
        /// <returns></returns>
        Accounts AccountsLogin(Accounts acc);
        /// <summary>
        /// ������֤��¼
        /// </summary>
        /// <param name="phone">�ֻ���</param>
        /// <param name="vcode">У����</param>
        /// <returns></returns>
        Accounts AccountsLoginSms(string phone, string vcode);

        /// <summary>
        /// ���ڼ�¼ÿ�ε�¼���ɵ���֤�룬���ڣ�ͬһ�˺ŵ�¼ʱ����ǰ�˺�����
        /// </summary>
        /// <param name="acid">�˺�ID</param>
        /// <param name="code">��¼״̬��״̬��</param>
        /// <returns></returns>
        void RecordLoginCode(int acid, string code);
        /// <summary>
        /// �ж��˺��Ƿ����
        /// </summary>
        /// <param name="accname">�˺���</param>
        /// <returns></returns>
        Accounts IsAccountsExist(string accname);
        /// <summary>
        /// �ж��˺��Ƿ����
        /// </summary>
        /// <param name="accname">�˺�����</param>
        /// <param name="id">�˺ŵ�id����������˺�����д0��С��0��ֵ</param>
        /// <returns>trueΪ���ڣ�falseΪ������</returns>
        bool IsAccountExist(string accname, int id);
        /// <summary>
        /// ��ǰ���ʺŻ���Ϣ�Ƿ��ظ�
        /// </summary>
        /// <param name="acid">��ǰ�˺ŵ�id</param>
        /// <param name="name">�˺ţ����ֻ���</param>
        /// <param name="type">�ж����ͣ�Ĭ��Ϊ�˺ţ�1Ϊ�ֻ���,2Ϊ����</param>
        /// <returns></returns>
        Accounts IsAccountsExist(int acid, string name, int type);
        /// <summary>
        /// �ж��˻��Ƿ��Ѿ��ڴ棬���ж��˺����ֻ���
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="enity"></param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, Accounts enity);
        /// <summary>
        /// ��ǰ���ʺ��Ƿ�����
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="accname"></param>
        /// <param name="answer">��ȫ�����</param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, string accname, string answer);
        /// <summary>
        /// ��ȡ�˻�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Accounts[] AccountsCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// ��ȡ�˻���Ϣ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="sts">�����id�����id�ö��ŷָ�</param>
        /// <param name="count">ȡ��������С��1Ϊ����</param>
        /// <returns></returns>
        List<Accounts> AccountsCount(int orgid, bool? isUse, string sts, int count);
        /// <summary>
        /// �����ж����˻�
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="isUse"></param>
        /// <param name="gender">�Ա�</param>
        /// <returns></returns>
        int AccountsOfCount(int orgid, bool? isUse, int gender);
        /// <summary>
        /// ��ҳ��ȡ���е���վ�˻��ʺţ�
        /// </summary>
        /// <param name="orgid">����ID</param>
        /// <param name="isUse"></param>
        /// <param name="size">ÿҳ��ʾ������¼</param>
        /// <param name="index">��ǰ�ڼ�ҳ</param>
        /// <param name="countSum">��¼����</param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, bool? isUse, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡĳ�˻��飬���е���վ�˻��ʺţ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sortid">ѧԱ����id</param>
        /// <param name="isuse">�Ƿ�����</param>
        /// <param name="acc">�˻�����</param>
        /// <param name="name">�������ǳ�</param>
        /// <param name="phone">�ֻ���</param>
        /// <param name="idcard">���֤��</param>
        /// <param name="gender">�Ա�,0Ϊ���У�1Ϊ�У�2ΪŮ</param>
        /// <param name="orderby">�����ֶ�</param>
        /// <param name="orderpattr">����ʽ��asc��desc</param>        
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, long sortid, bool? isuse, string acc,string name, string phone, string idcard, int gender, string orderby, string orderpattr, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡĳ�˻��飬���е���վ�˻��ʺţ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sortid">ѧԱ����id</param>
        /// <param name="pid">�Ƽ���id</param>
        /// <param name="isuse">�Ƿ�����</param>
        /// <param name="acc">�˻�����</param>
        /// <param name="name">�������ǳ�</param>
        /// <param name="phone">�ֻ���</param>
        /// <param name="idcard">���֤��</param>
        /// <param name="gender">�Ա�,0Ϊ���У�1Ϊ�У�2ΪŮ</param>
        /// <param name="orderby">�����ֶ�</param>
        /// <param name="orderpattr">����ʽ��asc��desc</param> 
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, long sortid, int pid, bool? isuse, string acc, string name, string phone,string idcard, int gender,string orderby, string orderpattr, int size, int index, out int countSum);
        /// <summary>
        /// ѧԱ�˺���Ϣ����
        /// </summary>
        /// <param name="path">�����ļ���·�����������ˣ�</param>
        /// <param name="orgid">����id</param>
        /// <param name="sorts">ѧԱ����id���ö��ŷָ�</param>
        /// <param name="isall">��sortsΪ��ʱ��true�����У�false��δ�����</param>
        /// <returns></returns>
        string AccountsExport4Excel(string path, int orgid, string sorts,bool isall);
        /// <summary>
        /// ѧԱ�˻��ŵ���
        /// </summary>
        /// <param name="path">�����ļ���·�����������ˣ�</param>
        /// <param name="orgs">����id,�ö��ŷָ�</param>
        /// <returns></returns>
        string AccountsExport4Excel(string path, string orgs);
        #endregion

        #region ������ƽ̨��
        /// <summary>
        /// �󶨵�����ƽ̨�˺ŵ�openid
        /// </summary>
        /// <param name="acid">��ϵͳ�˺�id</param>
        /// <param name="openid">������ƽ̨�˺ŵ�openid</param>
        /// <param name="nickname"></param>
        /// <param name="headurl"></param>
        /// <param name="field">��ϵͳ�˺ż�¼openid���ֶ���������������config.js�е�tag,��û���ֶ�ǰ׺Ac_</param>
        /// <returns></returns>
        Accounts BindThirdparty(int acid, string openid,string nickname,string headurl, string field);
        /// <summary>
        ///  �󶨵�����ƽ̨�˺ŵ�openid
        /// </summary>
        /// <param name="acc">��ϵͳ�˺��˺�</param>
        /// <param name="openid">������ƽ̨�˺ŵ�openid</param>
        /// <param name="nickname"></param>
        /// <param name="headurl"></param>
        /// <param name="field">��ϵͳ�˺ż�¼openid���ֶ���������������config.js�е�tag,��û���ֶ�ǰ׺Ac_</param>
        /// <returns></returns>
        Accounts BindThirdparty(Song.Entities.Accounts acc, string openid, string nickname, string headurl, string field);
        /// <summary>
        /// ��������ƽ̨�˺ŵ�openid
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Accounts UnBindThirdparty(int acid, string field);
        /// <summary>
        /// ��������ƽ̨�˺ŵ�openid
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Accounts UnBindThirdparty(Song.Entities.Accounts acc, string field);
        /// <summary>
        /// ��ȡ������ƽ̨�˺�
        /// </summary>
        /// <param name="acid">�˺�id</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        ThirdpartyAccounts ThirdpartyAccount(int acid, string tag);
        #endregion

        #region �¼��˻����ϼ��˻�
        /// <summary>
        /// �¼���Ա����
        /// </summary>
        /// <param name="acid">��ǰ�˺�ID</param>
        /// <param name="isAll">�Ƿ���������¼���true�����У�falseֻȡֱ���¼�</param>
        /// <returns></returns>
        int SubordinatesCount(int acid, bool isAll);
        /// <summary>
        /// �¼���Ա��ҳ��ȡ
        /// </summary>
        /// <param name="acid">��ǰ�˺�id</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="acc">�˺�</param>
        /// <param name="name">����</param>
        /// <param name="phone">�ֻ���</param>
        /// <param name="size">��ǰҳ������</param>
        /// <param name="index">�ڼ�ҳ</param>
        /// <param name="countSum">�ܼ�¼��</param>
        /// <returns></returns>
        Accounts[] SubordinatesPager(int acid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ�˻������и����˻�����������
        /// </summary>
        /// <param name="accid">��ǰ�˻�id</param>
        /// <returns></returns>
        Accounts[] Parents(int accid);
        /// <summary>
        /// ��ǰ�˻������и����˻�����������
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        Accounts[] Parents(Accounts acc);
        #endregion

        #region ���ֹ���
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        PointAccount PointAdd(PointAccount entity);
        /// <summary>
        /// ���ӵ�¼����
        /// </summary>
        /// <param name="acc">ѧԱ�˻�</param>
        /// <returns></returns>
        /// <returns>�˴ε�¼�����ӵĻ�����</returns>
        void PointAdd4Login(Accounts acc);
        /// <summary>
        /// ���ӵ�¼����
        /// </summary>
        /// <param name="accid">ѧԱ�˻�id</param>
        /// <param name="source">��Դ��Ϣ</param>
        /// <param name="info">��Ϣ</param>
        /// <param name="remark">��ע</param>
        /// <returns>�˴ε�¼�����ӵĻ�����</returns>
        void PointAdd4Login(int accid,string source,string info,string remark);
        /// <summary>
        /// ���ӷ������ӵķ��ʻ���
        /// </summary>
        /// <param name="acc">�˺Ŷ����ʵ��</param>
        /// <returns></returns>
        void PointAdd4Share(Accounts acc);
        /// <summary>
        /// ���ӷ������ӵ�ע�����
        /// </summary>
        /// <param name="acc">�˺Ŷ����ʵ��</param>
        /// <returns></returns>
        void PointAdd4Register(Accounts acc);
        /// <summary>
        /// ֧��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        PointAccount PointPay(PointAccount entity);
        /// <summary>
        /// ɾ����ˮ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PointDelete(PointAccount entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void PointDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        PointAccount PointSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰���ˮ��
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        PointAccount PointSingle(string serial);
        /// <summary>
        /// �޸���ˮ��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        void PointSave(PointAccount entity);
        /// <summary>
        /// ��ȡָ�������ļ�¼
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        PointAccount[] PointCount(int orgid, int stid, int type, int count);
        /// <summary>
        /// ����ĳһ��ʱ������Ļ���
        /// </summary>
        /// <param name="acid">ѧԱ�˻�</param>
        /// <param name="formType">��Դ���࣬1��¼��2������ʣ�3����ע�᣻4�һ�; </param>
        /// <param name="start">��ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <returns></returns>
        int PointClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        PointAccount[] PointPager(int orgid, int stid, int type, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="st">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="searTxt">����Ϣ����</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        PointAccount[] PointPager(int orgid, int st, int type, string searTxt, DateTime? start, DateTime? end, int size, int index, out int countSum);        
        #endregion

        #region ��ȯ����
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        CouponAccount CouponAdd(CouponAccount entity);
        /// <summary>
        /// ֧��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        CouponAccount CouponPay(CouponAccount entity);
        /// <summary>
        /// ���ֶһ���ȯ
        /// </summary>
        /// <param name="accid">ѧԱid</param>
        /// <param name="coupon">Ҫ�һ��Ŀ�ȯ����</param>
        /// <returns></returns>
        void CouponExchange(int accid,int coupon);
        /// <summary>
        /// ���ֶһ���ȯ
        /// </summary>
        /// <param name="acc">ѧԱ</param>
        /// <param name="coupon">Ҫ�һ��Ŀ�ȯ����</param>
        /// <returns>��ȯ���ӵ���ˮ��</returns>
        CouponAccount CouponExchange(Accounts acc, int coupon);
        /// <summary>
        /// ɾ����ˮ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CouponDelete(CouponAccount entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void CouponDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        CouponAccount CouponSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰���ˮ��
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        CouponAccount CouponSingle(string serial);
        /// <summary>
        /// �޸���ˮ��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        void CouponSave(CouponAccount entity);
        /// <summary>
        /// ��ȡָ�������ļ�¼
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="st">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        CouponAccount[] CouponCount(int orgid, int stid, int type, int count);
        /// <summary>
        /// ����ĳһ��ʱ������Ļ���
        /// </summary>
        /// <param name="acid">ѧԱ�˻�</param>
        /// <param name="formType">��Դ���࣬1�һ���2����֧����5����4����Ա��ֵ��</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CouponClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        CouponAccount[] CouponPager(int orgid, int stid, int type, int size, int index, out int countSum);
        CouponAccount[] CouponPager(int orgid, int stid, int type, DateTime? start, DateTime? end, string search, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���еĹ��棻
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="st">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="searTxt">����Ϣ����</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        CouponAccount[] CouponPager(int orgid, int st, int type, string searTxt, int size, int index, out int countSum);
        #endregion

        #region �ʽ����
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        MoneyAccount MoneyIncome(MoneyAccount entity);
        /// <summary>
        /// ֧��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        MoneyAccount MoneyPay(MoneyAccount entity);
        /// <summary>
        /// ͨ����ˮ�Ž����ʽ�֧���������ȷ�ϲ���
        /// </summary>
        /// <param name="serial">��ˮ��</param>
        /// <returns></returns>
        MoneyAccount MoneyConfirm(string serial);
        /// <summary>
        /// ͨ�����׼�¼�Ķ��󣬽����ʽ�֧���������ȷ�ϲ���
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        MoneyAccount MoneyConfirm(MoneyAccount ma);        
        /// <summary>
        /// ɾ����ˮ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void MoneyDelete(MoneyAccount entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void MoneyDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        MoneyAccount MoneySingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰���ˮ��
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        MoneyAccount MoneySingle(string serial);
        /// <summary>
        /// ĳ��ѧԱ���ʽ�����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="acid">�˺�id</param>
        /// <param name="type">1֧����2���루������ֵ������ȣ�</param>
        /// <param name="from">���ͣ���Դ��1Ϊ����Ա������2Ϊ��ֵ���ֵ��3������֧����4����γ�,5����</param>
        /// <param name="start">ʱ���������ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <returns></returns>
        decimal MoneySum(int orgid, int acid, int type, int from, DateTime? start, DateTime? end);
        /// <summary>
        /// ������ͳ���ʽ�
        /// </summary>
        /// <param name="interval">ʱ������YEAR:����ͳ��,MONTH:���£�WEEK:���ܣ�Day:����</param>
        /// <param name="orgid">����id</param>
        /// <param name="acid">�˺�id</param>
        /// <param name="type">1֧����2���루������ֵ������ȣ�</param>
        /// <param name="from">���ͣ���Դ��1Ϊ����Ա������2Ϊ��ֵ���ֵ��3������֧����4����γ�,5����</param>
        /// <param name="start">ʱ���������ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <returns></returns>
        Dictionary<string,double> MoneyStatistics(string interval,int orgid, int acid, int type, int from, DateTime? start, DateTime? end);
        /// <summary>
        /// ���ֵ�����ѹ���ѧԱ����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="type">1֧����2���루������ֵ������ȣ�</param>
        /// <param name="from">���ͣ���Դ��1Ϊ����Ա������2Ϊ��ֵ���ֵ��3������֧����4����γ�,5����</param>
        /// <returns></returns>
        int MoneyForAccount(int orgid, int type, int from);
        /// <summary>
        /// ��ֵ���ʽ���
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        decimal MoneyForTotal(int orgid, int type, int from);
        /// <summary>
        /// �޸���ˮ��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        void MoneySave(MoneyAccount entity);
        /// <summary>
        /// ��ȡָ�������ļ�¼
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="isSuccess">�Ƿ�����ɹ�</param>
        /// <param name="count"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyCount(int orgid, int stid, int type, bool? isSuccess, int count);
        /// <summary>
        /// ����ĳһ��ʱ��������ֽ�
        /// </summary>
        /// <param name="acid">ѧԱ�˻�</param>
        /// <param name="formType">1Ϊ����Ա������2Ϊ��ֵ���ֵ��3����֧����4����γ�,5����</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int MoneyClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// ��ҳ��ȡ�ʽ���ˮ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int stid, int type, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ�ʽ���ˮ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="from">��Դ��1Ϊ����Ա��2Ϊ��ֵ�룬3Ϊ����֧��</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="search">����Ϣ����</param>
        /// <param name="moneymin"></param>
        /// <param name="moneymax"></param>
        /// <param name="serial"></param>
        /// <param name="state">״̬��-1Ϊ���У�1Ϊ�ɹ���2Ϊʧ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int stid, int type, int from, DateTime? start, DateTime? end, string search, 
            int moneymin, int moneymax, string serial, int state, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ�ʽ���ˮ��
        /// </summary>
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="from">��Դ��1Ϊ����Ա��2Ϊ��ֵ�룬3Ϊ����֧��</param>
        /// <param name="account">ѧԱ���ƻ��˺�</param>
        /// <param name="start">��ʱ��������䣬��Ϊ��ʼʱ��</param>
        /// <param name="end">��ʱ��������䣬��Ϊ����ʱ��</param>
        /// <param name="state">״̬��-1Ϊ���У�1Ϊ�ɹ���2Ϊʧ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int type, int from, string account, DateTime? start, DateTime? end,
            int moneymin, int moneymax, string serial, int state,
            int size, int index, out int countSum);
        /// <summary>
        /// ����excel
        /// </summary>
        /// <param name="path">�洢·��</param>
        /// <param name="orgid">����������</param>
        /// <param name="acid">��ѧԱ������������ѧԱ��id</param>
        /// <param name="type">���ͣ�֧��Ϊ1��ת��2</param>
        /// <param name="from">��Դ��1Ϊ����Ա��2Ϊ��ֵ�룬3Ϊ����֧��</param>
        /// <param name="start">��ʱ��������䣬��Ϊ��ʼʱ��</param>
        /// <param name="end">��ʱ��������䣬��Ϊ����ʱ��</param>
        /// <returns></returns>
        string MoneyRecords4Excel(string path, int orgid, int[] acid, int type, int from, DateTime? start, DateTime? end);
        #endregion

        #region ���Գɼ�

        #endregion

        #region ͳ������
        /// <summary>
        /// ͳ�Ƹ�������ε�ѧԱ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="interval">����������ĳ�������</param>
        /// <returns></returns>
        DataTable AgeGroup(int orgid, int interval);
        /// <summary>
        /// ͳ��ѧԱע�������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="interval">�����λ��yΪ��,mΪ��,dΪ��</param>
        /// <param name="start">ͳ���������ʼʱ��</param>
        /// <param name="end">ͳ������Ľ���ʱ��</param>
        /// <returns></returns>
        DataTable RegTimeGroup(int orgid, string interval, DateTime start, DateTime end);
        /// <summary>
        /// ͳ��ѧԱ��¼���
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="interval">�����λ��yΪ��,mΪ��,dΪ��</param>
        /// <param name="start">ͳ���������ʼʱ��</param>
        /// <param name="end">ͳ������Ľ���ʱ��</param>
        /// <returns></returns>
        DataTable LoginTimeGroup(int orgid, string interval, DateTime start, DateTime end);
        #endregion
    }
}
