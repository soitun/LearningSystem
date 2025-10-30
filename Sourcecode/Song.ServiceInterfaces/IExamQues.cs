using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 在线考试中的试题管理
    /// </summary>
    public interface IExamQues : WeiSha.Core.IBusinessInterface
    {
        #region 试题
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        int QuesDelete(Questions entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int QuesDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int QuesRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int QuesRemove(long id);
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Questions> QuesPager(int orgid, long[] qpid, long[] tagid, long[] knlid, int[] type, int[] diff, int size, int index, out int countSum);
        #endregion

        #region 试题分类
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        int PartAdd(QuesPart entity);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        QuesPart PartIsExist(int orgid, long pid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        QuesPart PartIsExist(QuesPart entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PartSave(QuesPart entity);
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        int PartUpdate(long qpid, Field[] fields, object[] objs);
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        int PartUpdate(long qpid, Field field, object obj);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int PartDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int PartRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int PartRemove(long id);
        /// <summary>
        /// 清空试题分类下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qpid"></param>
        void PartClear(long qpid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesPart PartSingle(long id);
        /// <summary>
        /// 当前试题分类下的所有子试题分类id
        /// </summary>
        /// <param name="qpid">当前试题分类id</param>
        /// <param name="orgid">试题分类所属机构的ID,如果小于等于零，则取从数据库读取qpid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        List<long> PartTreeID(long qpid, int orgid);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题分类的id</param>
        /// <returns></returns>
        string PartName(long id);
        /// <summary>
        /// 当前试题分类，是否有子试题分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题分类Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        bool PartIsChildren(int orgid, long id, bool? isUse);
        /// <summary>
        /// 获取试题分类
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesPart> PartCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count);
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid">当前试题分类的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        List<QuesPart> PartParents(long qpid, bool isself);
        /// <summary>
        /// 计算试题分类的数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        int PartOfCount(int orgid, long pid, bool? isUse, bool children);
        /// <summary>
        /// 当前试题分类下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> PartQuestions(int orgid, long qpid, int qtype, bool? isUse,bool children,  int count);
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int PartQusTotal(int orgid, long qpid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesPart> PartPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">试题分类列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        bool PartUpdateTaxis(QuesPart[] list);
        #endregion

        #region 收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        bool CollectAdd(int accid, long qusid);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        bool CollectRemove(int accid, long qusid);
        /// <summary>
        /// 批量取消收藏
        /// </summary>
        int CollectRemove(int accid, long[] qusid);
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        bool Collected(int accid, long qusid);
        /// <summary>
        /// 获取收藏的试题
        /// </summary>
        /// <param name="acid">管理员id</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">试题标签id</param>
        /// <param name="knlid">试题知识点id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="size">分页大小</param>
        /// <param name="index">分页索引</param>
        /// <param name="countSum">总记录数</param>
        /// <returns></returns>
        List<Questions> CollectPager(int acid, long[] qpid, long[] tagid, long[] knlid, int[] type, int[] diff, int size, int index, out int countSum);
        #endregion

        #region 知识点
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">业务实体</param>
        int KnlAdd(QuesKnowledge entity);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        QuesKnowledge KnlIsExist(int orgid, long pid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        QuesKnowledge KnlIsExist(QuesKnowledge entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void KnlSave(QuesKnowledge entity);
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        int KnlUpdate(long qkid, Field[] fields, object[] objs);
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        int KnlUpdate(long qkid, Field field, object obj);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int KnlDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int KnlRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int KnlRemove(long id);
        /// <summary>
        /// 清空试题知识点下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qkid"></param>
        void KnlClear(long qkid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesKnowledge KnlSingle(long id);
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id
        /// </summary>
        /// <param name="qkid">当前试题知识点id</param>
        /// <param name="orgid">试题知识点所属机构的ID,如果小于等于零，则取从数据库读取qkid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        List<long> KnlTreeID(long qkid, int orgid);
        /// <summary>
        /// 获取试题知识点名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题知识点的id</param>
        /// <returns></returns>
        string KnlName(long id);
        /// <summary>
        /// 当前试题知识点，是否有子试题知识点
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题知识点Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        bool KnlIsChildren(int orgid, long id, bool? isUse);
        /// <summary>
        /// 获取试题知识点
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesKnowledge> KnlCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count);
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid">当前试题知识点的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        List<QuesKnowledge> KnlParents(long qkid, bool isself);
        /// <summary>
        /// 计算试题知识点的数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        int KnlOfCount(int orgid, long pid, bool? isUse, bool children);
        /// <summary>
        /// 当前试题知识点下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> KnlQuestions(int orgid, long qkid, int qtype, bool? isUse, bool children, int count);
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int KnlQusTotal(int orgid, long qkid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesKnowledge> KnlPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">试题知识点列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        bool KnlUpdateTaxis(QuesKnowledge[] list);
        #endregion

        #region 关键字
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int TagAdd(QuesTags entity);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TagIsExist(int orgid, long couid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TagIsExist(QuesTags entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TagSave(QuesTags entity);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int TagDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int TagRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int TagRemove(long id);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesTags TagSingle(long id);
        /// <summary>
        /// 获取试题标签
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesTags> TagCount(int orgid, string sear, long couid, bool? isdeleted, int count);
        /// <summary>
        /// 计算试题标签的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param> 
        /// <returns></returns>
        int TagOfCount(int orgid, long couid, bool? isdeleted);
        /// <summary>
        /// 当前试题标签下的所有试题
        /// </summary>
        /// <param name="qtagid"></param>
        /// <param name="couid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> TagQuestions(long qtagid, long couid, int qtype, bool? isuse, int count);
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <returns></returns>
        int TagQusTotal(long qtagid, long couid, int qtype, bool? isuse);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesTags> TagPager(int orgid, long couid, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 回收站
        #endregion
    }
}
