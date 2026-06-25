using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeiSha.Data;

namespace Song.ViewData
{
    /// <summary>
    /// 时间对象的扩展
    /// </summary>
    public static class DataTimeExtensions
    {
        /// <summary>
        /// 转为JS时间字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToJsString(this DateTime date)
        {
            System.DateTime time = System.DateTime.Now;
            if (date != null) time = Convert.ToDateTime(date);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            //将C#时间转换成JS时间字符串    
            return string.Format("/Date({0})/", timeStamp);          
        }
        /// <summary>
        /// 转为C#时间
        /// </summary>
        public static DateTime ToDateTime(this string str)
        {
            if(string.IsNullOrWhiteSpace(str))return DateTime.MinValue;
            // 尝试多种日期格式
            var formats = new[] {"yyyy/MM/dd HH:mm:ss","yyyy-MM-dd HH:mm:ss","yyyy/MM/dd 星期日 HH:mm:ss",
                "yyyy/MM/dd","yyyy-MM-dd","MM/dd/yyyy HH:mm:ss","M/d/yyyy HH:mm:ss"};

            if (DateTime.TryParseExact(str, formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result))
            {
                return result;
            }
            else
            {
                //如果字符为 2023-01-07 16:20:08 的格式
                string patternWithGroups = @"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$";
                Match match = Regex.Match(str, patternWithGroups);
                if (match.Success)
                {
                    return DateTime.Parse(str);
                }
            }
            // 尝试时间戳格式
            if (str.EndsWith(".0000"))
            {
                DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                if (str.IndexOf(".") > -1) str = str.Substring(0, str.IndexOf("."));
                long lTime = long.Parse(str + "0000");
                return dt.Add(new TimeSpan(lTime));
            }
            // 最后尝试通用解析
            return DateTime.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}
