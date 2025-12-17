using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 将字符串转为所需类型
    /// </summary>
    public class StringTo
    {  
        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T[] Array<T>(string str) where T : struct
        {           
            List<T> list = StringTo.List<T>(str);          
            return list.ToArray<T>();
        }
        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> List<T>(string str) where T : struct
        {           
            List<T> list = new List<T>();
            if (string.IsNullOrWhiteSpace(str)) return list;
            string[] arr = str.Split(',');
            foreach (string s in arr)
            {
                if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(s.Trim())) continue;
                T t = (T)System.Convert.ChangeType(s, typeof(T));
                list.Add(t);
            }
            return list;
        }
        public static T Number<T>(object obj)
        {
            T t = (T)System.Convert.ChangeType(obj, typeof(T));
            return t;
        }
    }
}
