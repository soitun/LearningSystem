using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ��ʽ�Ĺ���
    /// </summary>
    public interface IStyle : WeiSha.Core.IBusinessInterface
    {
        #region ��������
        /// <summary>
        /// ��ӵ�����Ŀ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NaviAdd(Navigation entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NaviSave(Navigation entity);
        /// <summary>
        /// �޸ĵ�������ʾ״̬
        /// </summary>
        /// <param name="id">����id</param>
        /// <param name="show">�Ƿ���ʾ</param>
        /// <returns></returns>
        bool NaviState(int id, bool show);
        /// <summary>
        /// �����޸ĵ�����ͼƬ��ַ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logo"></param>
        void NaviSaveLogo(Navigation entity, string logo);
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void NaviDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Navigation NaviSingle(int identify);
        /// <summary>
        ///  ��ȡ��һʵ����󣬰�Uid��
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Navigation NaviSingle(string uid);
        /// <summary>
        /// ��ȡ���е���
        /// </summary>
        /// <param name="isShow">�Ƿ���ǰ̨��ʾ</param>
        /// <param name="site">վ����࣬��ҵվweb���ֻ�վmobi��΢��վweixin��Ĭ��Ϊweb</param>
        /// <param name="type">ĳһ�ർ��</param>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid);
        List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid, string pid);
        /// <summary>
        /// ��ǰ������¼�����
        /// </summary>
        /// <param name="pid">����id</param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <returns></returns>
        Navigation[] NaviChildren(string pid, bool? isShow);
        /// <summary>
        /// ���µ����˵���
        /// </summary>
        /// <param name="site"></param>
        /// <param name="type"></param>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateNavigation(string site, string type, int orgid,string pid, Navigation[] items);
        #endregion

        #region �ֻ�ͼƬ����
        /// <summary>
        /// ����ֻ�ͼƬ
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ShowPicAdd(ShowPicture entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ShowPicSave(ShowPicture entity);
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void ShowPicDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        ShowPicture ShowPicSingle(int identify);
        /// <summary>
        /// ��ȡ�ֻ�ͼƬ
        /// </summary>
        /// <param name="isShow">�Ƿ���ǰ̨��ʾ</param>
        /// <param name="site">վ����࣬��ҵվweb���ֻ�վmobi��΢��վweixin��Ĭ��Ϊweb</param>       
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        ShowPicture[] ShowPicAll(bool? isShow, string site, int orgid);
        /// <summary>
        /// <summary>
        /// ����˳��
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool ShowUpdateTaxis(ShowPicture[] items);       
        #endregion
    }
}
