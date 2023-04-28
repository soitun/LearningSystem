﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Song.ViewData
{
    public class Html
    {
        /// <summary>
        /// 理解HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearHTML(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]+?>[\s\S]*?</script>", "", option);
            html = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", "", option);
            //删除HTML
            html = Regex.Replace(html, @"<(.[^>]*)>", "", option);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", option);
            html = Regex.Replace(html, @"-->", "", option);
            html = Regex.Replace(html, @"<!--.*", "", option);
            //html = Regex.Replace(html, @"&(quot|#34);", "\"", option);
            //html = Regex.Replace(html, @"&(amp|#38);", "&", option);
            //html = Regex.Replace(html, @"&(lt|#60);", "<", option);
            //html = Regex.Replace(html, @"&(gt|#62);", ">", option);
            //html = Regex.Replace(html, @"&(nbsp|#160);", " ", option);
            //html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", option);
            //html = Regex.Replace(html, @"&(cent|#162);", "\xa2", option);
            //html = Regex.Replace(html, @"&(pound|#163);", "\xa3", option);
            //html = Regex.Replace(html, @"&(copy|#169);", "\xa9", option);
            //html = Regex.Replace(html, @"&#(\d+);", "", option);

            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", option);
            //html = html.Replace("<", "&lt;");
            //html = html.Replace(">", "&gt;");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            return html.Trim();
        }
        /// <summary>
        /// 清理Js脚本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearScript(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            //删除脚本
            html = Regex.Replace(html, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", option);
            html = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", "", option);
            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", option);
            //清理掉<!DOCTYPE，主要是为了防止XXE攻击
            html = Regex.Replace(html, @"<(\s*)!DOCTYPE[^>]*>", "", option);
            html = Regex.Replace(html, @"<(\s*)!ENTITY[^>]*>", "", option);

            return html;
        }
        /// <summary>
        /// 清除指定的html标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static string ClearHTML(string html, params string[] elements)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option=RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            foreach (string el in elements)
            {
                html = Regex.Replace(html, @"<" + el + @"[^>]*>", "", option);
                html = Regex.Replace(html, @"</" + el + @"[^>]*>", "", option);
            }           
            return html.Trim();
        }
        /// <summary>
        /// 清理HTML标签中的属性
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tag">清除指定的标签中的属性</param>
        /// <returns></returns>
        public static string ClearAttr(string html, string tag)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            return Regex.Replace(html, @"<(" + tag + @")\s*[^><]*>", @"<$1>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        }
        /// <summary>
        /// 清理HTML标签中的属性
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string ClearAttr(string html, params string[] tags)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            foreach (string tag in tags)
                html = ClearAttr(html, tag);
            return html;
        }
        /// <summary>
        /// 清理掉<!DOCTYPE，主要是为了防止XXE攻击
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearDoctype(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            html = Regex.Replace(html, @"<(\s*)!DOCTYPE[^>]*>", "", option);
            html = Regex.Replace(html, @"<(\s*)!ENTITY[^>]*>", "", option);
            return html;
        }
    }
}
