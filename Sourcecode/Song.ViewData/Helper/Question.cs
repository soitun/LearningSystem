using Newtonsoft.Json.Linq;
using Song.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 试题相关的处理方法
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 将试题的答题选项(Json)转换为数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<Song.Entities.QuesAnswer> AnswerToItems(Song.Entities.Questions entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Qus_Items)) return null;
            List<Song.Entities.QuesAnswer> items = new List<QuesAnswer>();
            JArray jaryy = JArray.Parse(entity.Qus_Items);
            if (jaryy != null)
            {
                for (int i = 0; i < jaryy.Count; i++)
                {
                    JToken jt = jaryy[i];
                    try
                    {
                        Song.Entities.QuesAnswer obj = ExecuteMethod.ValueToEntity<Song.Entities.QuesAnswer>(null, jt.ToString());
                        if (string.IsNullOrWhiteSpace(obj.Ans_Context)) continue;
                        //生成答案项的id
                        if (obj.Ans_ID <= 0) obj.Ans_ID = WeiSha.Core.Request.SnowID();
                        //填空题，每项都是正确的
                        if (entity.Qus_Type == 5)
                        {
                            obj.Ans_IsCorrect = true;
                            obj.Ans_Context = HTML.ClearTag(obj.Ans_Context);
                        }
                        items.Add(obj);
                    }
                    catch { }
                }
            }
            return items;
        }
    }
}
