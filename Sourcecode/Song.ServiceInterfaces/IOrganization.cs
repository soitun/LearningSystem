using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ժϵְλ�Ĺ���
    /// </summary>
    public interface IOrganization : WeiSha.Core.IBusinessInterface
    {
        #region ��������
        /// <summary>
        /// ��ӻ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void OrganAdd(Organization entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void OrganSave(Organization entity);
        /// <summary>
        /// ����Ĭ�ϻ���
        /// </summary>
        /// <returns></returns>
        void OrganSetDefault(int identify);
        /// <summary>
        /// ϵͳĬ�ϲ��õĻ�����ע������Root����)
        /// </summary>
        /// <returns></returns>
        Organization OrganDefault();
        /// <summary>
        /// ����ϵͳ����Ļ�����ע����Root����)
        /// </summary>
        /// <returns></returns>
        Organization OrganRoot();
        /// <summary>
        /// ��ǰ����,ͨ�����������жϣ�����������򷵻�Ĭ�ϻ���
        /// </summary>
        /// <returns></returns>
        Organization OrganCurrent();
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        Organization OrganSingle(int identify);
        /// <summary>
        /// ��ǰ�����Ƿ�����
        /// </summary>
        /// <param name="name">����������</param>
        /// <param name="id">������id</param>   
        /// <returns></returns>
        bool ExistName(string name, int id);
        /// <summary>
        /// ����ƽ̨�����Ƿ��ظ�
        /// </summary>
        /// <param name="name">������ƽ̨����</param>
        /// <param name="id">������id</param>
        /// <returns></returns>
        bool ExistPlatform(string name, int id);
        /// <summary>
        /// �����Ķ�������ظ�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id">������id</param>
        /// <returns></returns>
        bool ExistDomain(string name, int id);
        /// <summary>
        /// ��������ɾ����˾��
        /// </summary>
        /// <param name="identify">����id</param>
        void OrganDelete(int identify);
        /// <summary>
        /// ȡ���л���
        /// </summary>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="level">�����ȼ�</param>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Organization> OrganAll(bool? isUse, int level, string search);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="isUse">�Ƿ�ʹ��</param>
        /// <param name="isShow">�Ƿ���ǰ����ʾ</param>
        /// <param name="level">�����ȼ�</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        List<Organization> OrganCount(bool? isUse, bool? isShow, int level, int count);
        /// <summary>
        /// ������ʱ�ļ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="day">���������֮ǰ��</param>
        void OrganClearTemp(int orgid, int day);
        /// <summary>
        /// ����ǰ����������
        /// </summary>
        /// <param name="orgid"></param>
        void OrganClear(int orgid);
        /// <summary>
        /// ��������
        /// </summary>
        List<Organization> OrganBuildCache();
        /// <summary>
        /// ��ҳ��ȡ����
        /// </summary>
        /// <param name="isUse">�Ƿ�ʹ��</param>
        /// <param name="level">�����ȼ�</param>
        /// <param name="searTxt">�������ƹؼ���</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Organization> OrganPager(bool? isUse, int level, string searTxt, int size, int index, out int countSum);

        #endregion

        #region �����ȼ�
        /// <summary>
        /// ��ӻ����ȼ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void LevelAdd(OrganLevel entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void LevelSave(OrganLevel entity);
        /// <summary>
        /// ����Ĭ�ϵȼ���Ĭ�ϵȼ�ֻ��һ��
        /// </summary>
        /// <param name="identify"></param>
        void LevelSetDefault(int identify);
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        OrganLevel LevelSingle(int identify);
        /// <summary>
        /// Ĭ�ϵĻ����ȼ�
        /// </summary>
        /// <returns></returns>
        OrganLevel LevelDefault();
        /// <summary>
        /// ��ȡ���ж���
        /// </summary>
        /// <returns></returns>
        OrganLevel[] LevelAll(string search, bool? isUse);
        /// <summary>
        /// ��ǰ�����ȼ��£��м�������
        /// </summary>
        /// <param name="lvid">�����ȼ�id</param>
        /// <returns></returns>
        int LevelOrganCount(int lvid);
        /// <summary>
        /// ��������ɾ����˾��
        /// </summary>
        /// <param name="identify">����id</param>
        bool LevelDelete(int identify);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="name">�����ȼ�������</param>
        /// <param name="id">�����ȼ���id</param>   
        /// <returns></returns>
        bool LevelNameExist(string name, int id);
        /// <summary>
        /// �����ȼ���tag��ʶ�Ƿ�����
        /// </summary>
        /// <param name="tag">�����ȼ���tag��ʶ</param>
        /// <param name="id">�����ȼ���id</param>   
        /// <returns></returns>
        bool LevelTagExist(string tag, int id);
        /// <summary>
        /// ���Ļ����ȼ�������
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool LevelUpdateTaxis(OrganLevel[] items);

        #endregion

        #region ͳ������
        /// <summary>
        /// �ж��ٿγ̱�ѡ�޹�
        /// </summary>
        /// <param name="orgid">����id</param>    
        /// <param name="isfree">�Ƿ������ѵ�</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CourseCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);
        /// <summary>
        /// �ж���ѧԱ������γ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isfree">�Ƿ������ѵ�</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int StudentCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);
        /// <summary>
        /// ����γ̵Ĵ�������ѧԱ����γ̵Ĵ���
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isfree">�Ƿ������ѵ�</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CourseSumBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);

        /// <summary>
        /// ����ͳ�����ݵĶ�ʱ����
        /// </summary>
        void UpdateStatisticalData_CronJob();
        /// <summary>
        /// ͳ�������ӳ�ִ��
        /// </summary>
        /// <param name="minute">�ӳٵķ�����</param>
        void UpdateStatisticalData_Delay(int minute);
        /// <summary>
        /// ���»�����ͳ������
        /// </summary>
        void UpdateStatisticalData();
        #endregion
    }
}
