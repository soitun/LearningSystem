using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ժϵְλ�Ĺ���
    /// </summary>
    public interface IPurview : WeiSha.Core.IBusinessInterface
    {
        #region �¼�
        /// <summary>
        /// ���˵���仯ʱ��������ɾ�ġ�Ȩ������
        /// </summary>
        event EventHandler OnChanged;

        #endregion
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(Purview entity);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="memberid">��Աid����Ȩ�޸�������id</param>
        /// <param name="mmids">����˵���id</param>
        /// <param name="type">��Ա����</param>
        void BatchAdd(int memberid, string[] mmids, string type);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(Purview entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(Purview entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ���ݷ��ࡢ����idɾ��
        /// </summary>
        /// <param name="memberId">��ɫ��Ժϵ�����id</param>
        /// <param name="type">�������ֲ�ͬȨ�޷��䣬"Posi"��ɫ(��λ)��"Group"�顢"Depart"Ժϵ</param>
        void Delete(int memberId, string type);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Purview GetSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�Ȩ�������id
        /// </summary>
        /// <param name="mmUid">���ܲ˵���uid</param>
        /// <param name="type">�������ֲ�ͬȨ�޷��䣬"Posi"��ɫ(��λ)��"Group"�顢"Depart"Ժϵ</param>
        /// <returns></returns>
        Purview GetSingle4Member(string mmUid,string type);
        /// <summary>
        /// ��ȡ���ж��󣬰�Ȩ�������id
        /// </summary>
        /// <param name="memberId">��Աid</param>
        /// <param name="type">�������ֲ�ͬȨ�޷��䣬"Posi"��ɫ(��λ)��"Group"�顢"Depart"Ժϵ</param>
        /// <returns></returns>
        Purview[] GetAll(int memberId,string type);
        ///// <summary>
        ///// ��ȡĳ��Ա����ӵ�õ�ȫ������Ȩ�ޣ����������顢������ɫ������Ժϵ������Ȩ��
        ///// </summary>
        ///// <param name="accid">Ա��id</param>
        ///// <returns></returns>
        //ManageMenu[] GetAll4Emplyee(int accid);
        ///// <summary>
        ///// ������Ȩ��(����Ȩ��������ȼ�Ȩ��Ϊ����)
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <returns></returns>
        //ManageMenu[] GetAll4Org(int orgid);
        ///// <summary>
        ///// ��ȡ������ĳһ�����˵����Ȩ��
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="marker">�����ʦ����teacher,ѧ������student,��������organAdmin</param>
        ///// <returns></returns>
        //ManageMenu[] GetAll4Org(int orgid, string marker);
        /// <summary>
        /// ��ȡ�����ȼ���Ȩ��
        /// </summary>
        /// <param name="lvid">�����ȼ���id</param>
        /// <returns></returns>
        Purview[] OrganLevelItems(int lvid);
        /// <summary>
        /// ��ȡĳ������ĳһ�����˵����Ȩ��
        /// </summary>
        /// <param name="org"></param>
        /// <param name="marker">�����ʦ����teacher,ѧ������student,��������organAdmin</param>
        /// <returns></returns>
        List<ManageMenu> GetOrganPurview(Song.Entities.Organization org, string marker);
    }
}
