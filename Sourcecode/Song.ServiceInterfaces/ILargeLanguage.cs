using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using System.Data.Common;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ������ģ�͵�ҵ��ӿ�
    /// </summary>
    public interface ILargeLanguage : WeiSha.Core.IBusinessInterface
    {        
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void RecordsAdd(LlmRecords entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void RecordsSave(LlmRecords entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void RecordsDelete(int identify);
        /// <summary>
        /// ����ѧԱ���м�¼
        /// </summary>
        /// <param name="acid"></param>
        int RecordsClear(int acid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        LlmRecords RecordsSingle(int identify);
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="acid">ѧԱ���˺�id</param>
        /// <returns></returns>
        List<LlmRecords> RecordsAll(int acid);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="acid">ѧԱ���˺�id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<LlmRecords> RecordsPager(int acid, int size, int index, out int countSum);
    }
}
