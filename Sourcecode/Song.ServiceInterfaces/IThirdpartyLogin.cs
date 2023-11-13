using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ��������¼����
    /// </summary>
    public interface IThirdpartyLogin : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ͨ��tag��ǩ��ȡ��¼������
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        ThirdpartyLogin GetSingle(string tag);
        /// <summary>
        /// ͨ��id��¼������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ThirdpartyLogin GetSingle(int id);
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity"></param>
        void Save(ThirdpartyLogin entity);
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="isuse"></param>
        /// <returns></returns>
        List<ThirdpartyLogin> GetAll(bool? isuse);
    }
}
