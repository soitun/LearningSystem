using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试所用的试卷
    /// </summary>
    public class ExamTestPaper : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        private static string _pathKey = "ExamTestPaper";

        private static string _virPath = WeiSha.Core.Upload.Get[_pathKey].Virtual;
        private static string _phyPath = WeiSha.Core.Upload.Get[_pathKey].Physics;

        #region 增删改查

        /// <summary>
        /// 获取试卷信息
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.ExamTestPaper ForID(long id)
        {
            Song.Entities.ExamTestPaper tp = Business.Do<IExamTestPaper>().PaperSingle(id);
            if (tp == null) throw new Exception("试卷不存在！");
            return _tran(tp);
        }
        /// <summary>
        /// 获取试卷信息详情，包括抽题范围
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <returns>paper:试卷对象，parts:试题分类,knls:知识点, tags:关键字，questions:试题题型分配</returns>
        [HttpGet]
        public JObject Details(long id)
        {
            Song.Entities.ExamTestPaper tp = Business.Do<IExamTestPaper>().PaperSingle(id);
            if (tp == null) throw new Exception("试卷不存在！");
            JObject jo=new JObject();
            jo.Add("paper", _tran(tp).ToJObject());

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(tp.Etp_FromConfig);
            //试题分类
            XmlNode nodeparts = xmldoc.SelectSingleNode("/testpaper/range/parts");
            List<QuesPart> parts = Business.Do<IExamQues>().PartSingle(Helper.StringTo.Array<long>(nodeparts.InnerText));
            jo.Add("parts", parts?.ToJArray());
            //关联的知识点
            XmlNode nodeknls = xmldoc.SelectSingleNode("/testpaper/range/knls");
            List<QuesKnowledge> knls = Business.Do<IExamQues>().KnlSingle(Helper.StringTo.Array<long>(nodeknls.InnerText));
            jo.Add("knls",  knls?.ToJArray());
            //关联的标签
            XmlNode nodetags = xmldoc.SelectSingleNode("/testpaper/range/tags");
            List<QuesTags> tags = Business.Do<IExamQues>().TagSingle(Helper.StringTo.Array<long>(nodetags.InnerText));
            jo.Add("tags", tags?.ToJArray());
            //选择范围的试题数量
            JObject joquescount=new JObject();
            long[] qpid = parts == null ? null : parts.Select(p => p.Qp_ID).ToArray();
            int partcount = Business.Do<IExamQues>().PartQusTotal(-1, qpid, -1, true, true);
            joquescount.Add("part", partcount);
            joquescount.Add("knl", Business.Do<IExamQues>().KnlQusTotal(-1, knls?.Select(p => p.Qk_ID).ToArray(), -1, true, true));
            joquescount.Add("tag", Business.Do<IExamQues>().TagQusTotal(tags?.Select(p => p.Qtag_ID).ToArray(), -1, -1,  true));
            joquescount.Add("total", 0);
            jo.Add("quescount", joquescount);
            //
            //各题型的占比
            XmlNodeList nodeitems = xmldoc.SelectNodes("/testpaper/questions/item");
            JArray joitems = new JArray();
            foreach (XmlNode item in nodeitems)
            {
                JObject joitem = new JObject();
                foreach(XmlAttribute att in item.Attributes)
                    joitem.Add(att.Name, att.Value);               
                joitems.Add(joitem);               
            }
            jo.Add("questions", joitems);
            return jo;
        }
        ///<summary>
        /// 创建试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Config = "TestPaperLogo")]
        [HtmlClear(Not = "entity")]
        public Song.Entities.ExamTestPaper Add(Song.Entities.ExamTestPaper entity)
        {
            //当前管理员
            EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) return null;
            entity.Acc_Id = acc.Acc_Id;
            entity.Acc_AccName= acc.Acc_AccName;

            string filename = string.Empty, smallfile = string.Empty;
            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(_phyPath + filename);
                break;
            }
            entity.Etp_Logo = filename;

            Business.Do<IExamTestPaper>().PaperAdd(entity);
            return entity;
        }

        /// <summary>
        /// 修改试卷信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Config = "TestPaperLogo")]
        [HtmlClear(Not = "entity")]
        public Song.Entities.ExamTestPaper Modify(Song.Entities.ExamTestPaper entity)
        {
            //当前管理员
            EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) return null;
            entity.Acc_Id = acc.Acc_Id;
            entity.Acc_AccName = acc.Acc_AccName;

            string filename = string.Empty, smallfile = string.Empty;
            Song.Entities.ExamTestPaper old = Business.Do<IExamTestPaper>().PaperSingle(entity.Etp_Id);
            if (old == null) throw new Exception("Not found entity for TestPaper！");
            //如果有上传文件
            if (this.Files.Count > 0)
            {
                //只保存第一张图片
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(_phyPath + filename);
                    break;
                }
                entity.Etp_Logo = filename;
                if (!string.IsNullOrWhiteSpace(old.Etp_Logo))
                    WeiSha.Core.Upload.Get[_pathKey].DeleteFile(old.Etp_Logo);
            }
            //如果没有上传图片，且新对象没有图片，则删除旧图
            else if (string.IsNullOrWhiteSpace(entity.Etp_Logo) && !string.IsNullOrWhiteSpace(old.Etp_Logo))
            {
                WeiSha.Core.Upload.Get[_pathKey].DeleteFile(old.Etp_Logo);
            }

            old.Copy<Song.Entities.ExamTestPaper>(entity);
            Business.Do<IExamTestPaper>().PaperSave(old);
            return old;
        }

        /// <summary>
        /// 删除试卷
        /// </summary>
        /// <param name="id">试卷id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamTestPaper>().PaperDelete(s);
            return i;           
        }
        /// <summary>
        /// 还原逻辑删除试题
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int QuesRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamTestPaper>().PaperRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题分
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int QuesRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = ViewData.Helper.StringTo.List<long>(id);
            foreach (long s in list)
                i += Business.Do<IExamTestPaper>().PaperRemove(s);
            return i;
        }
        /// <summary>
        /// 修改试卷的状态
        /// </summary>
        /// <param name="id">试卷的id，可以是多个，用逗号分隔</param>
        /// <param name="use">是否启用</param>
        /// <param name="rec">是否推荐</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public int ModifyState(string id, bool use, bool? rec)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    if (rec != null)
                    {
                        Business.Do<IExamTestPaper>().PaperUpdate(idval,
                        new WeiSha.Data.Field[] {
                        Song.Entities.ExamTestPaper._.Etp_IsUse,
                        Song.Entities.ExamTestPaper._.Etp_IsRec },
                        new object[] { use, rec });
                    }
                    else
                    {
                        Business.Do<IExamTestPaper>().PaperUpdate(idval, new WeiSha.Data.Field[] { Song.Entities.ExamTestPaper._.Etp_IsUse },
                            new object[] { use });
                    }
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }

        /// <summary>
        /// 前端的分页获取试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="seach">检索字符</param>
        /// <param name="diff">难度等级</param>
        /// <param name="use">是否启用</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        [HttpGet]
        public ListResult ShowPager(int orgid, string seach,  int diff, bool? use, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            int count;
            List<Song.Entities.ExamTestPaper> tps = Business.Do<IExamTestPaper>().PaperPager(orgid, -1, seach, false, diff, use, size, index, out count);
            for (int i = 0; i < tps.Count; i++)
                tps[i] = _tran(tps[i]);
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 分页获取所有试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accid">管理员id</param>
        /// <param name="seach">按名称检索</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="use"></param>
        /// <param name="diff">难度等级</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        [HttpGet]
        public ListResult Pager(int orgid, int accid, string seach, bool? isdeleted, int diff, bool? use, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            int count;
            List<Song.Entities.ExamTestPaper> tps = Business.Do<IExamTestPaper>()
                .PaperPager(orgid, accid, seach, isdeleted, diff, use, size, index, out count);
            for (int i = 0; i < tps.Count; i++)
                tps[i] = _tran(tps[i]);
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 处理试卷对象的一些信息
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        private Song.Entities.ExamTestPaper _tran(Song.Entities.ExamTestPaper paper)
        {
            if (paper == null) return paper;
            paper.Etp_Logo = System.IO.File.Exists(_phyPath + paper.Etp_Logo) ? _virPath + paper.Etp_Logo : "";   
            return paper;
        }

        #endregion 增删改查
    }
}
