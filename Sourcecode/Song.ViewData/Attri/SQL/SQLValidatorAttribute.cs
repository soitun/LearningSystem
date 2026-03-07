using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// SQL注入的敏感字符过滤
    /// </summary>
    public class SQLValidatorAttribute : WeishaAttr
    {    
        /// <summary>
        /// 不校验的参数名，多个用逗号隔开
        /// </summary>
        public string Not { get; set; }
        public SQLValidatorAttribute()
        {

        }
        /// <summary>
        /// 清理SQL注入的敏感字符
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <param name="letter">客户端传来的信息</param>
        /// <returns></returns>
        public static SQLValidatorAttribute Clear(MemberInfo method, Letter letter)
        {
            SQLValidatorAttribute attr = null;
            attr = WeishaAttr.GetAttr<SQLValidatorAttribute>(method);
            if (attr != null)
            {
                letter.Params = letter.Params.ToDictionary(x => x.Key, (x) => {
                    string[] nots = attr.Not.Split(',');
                    bool isexist = false;
                    foreach (string s in nots)
                    {
                        if (string.IsNullOrWhiteSpace(s)) continue;
                        if (s.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isexist = true;
                            break;
                        }
                    }
                    if (isexist) return _isSqlClear(x.Value);
                    return _isSqlClear(x.Value);
                });
            }
            else
            {
                letter.Params = letter.Params.ToDictionary(x => x.Key, x => _isSqlClear(x.Value));
            }
            return attr;
        }
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <returns>通过校验为true</returns>
        public static bool Verify(MethodInfo method, Letter letter)
        {
            SQLValidatorAttribute attr = WeishaAttr.GetAttr<SQLValidatorAttribute>(method);
            string[] nots = new string[] { };
            if (attr != null) nots = attr.Not.Split(',');
            foreach (ParameterInfo param in method.GetParameters())
            {
                if (param.ParameterType != typeof(string)) continue;    //不是字符串类型的跳过
                                                                        //明确不检测的跳过
                bool exists = nots.Any(s => s.IndexOf(param.Name, StringComparison.OrdinalIgnoreCase) >= 0);
                if (exists) continue;
                //检测是否存在危险字符
                string val = letter.GetParameter(param.Name).Value;
                if (_isSqlInjectionSafe(val)) throw new Exception($"参数 {param.Name} 的内容“{val}”存在危险字符");               
            }               
            return true;
        }
        /// <summary>
        /// Sql敏感字符串
        /// </summary>
        private static  readonly string[] sqlInjectionPatterns = {
            @"\bSELECT\b.*\bFROM\b",
            @"\bINSERT\b.*\bINTO\b",
            @"\bUPDATE\b.*\bSET\b",
            @"\bDELETE\b.*\bFROM\b",
            @"\bDROP\b.*\bTABLE\b",
            @"\bUNION\b.*\bSELECT\b",
            @"\bEXEC\b.*\(",
            @"\bEXECUTE\b.*\(",
            @"\bDECLARE\b.*@",
            @"\bCAST\b.*\(",
            @"--",
            @"';.*--",
            @"\bOR\b.*=",
            @"\bAND\b.*=",
            @"'.*\bOR\b.*'.*=",
            @"\bWAITFOR\b.*\bDELAY\b",
            @"\bBENCHMARK\b.*\("
        };
    
        /// <summary>
        /// 验证输入是否包含SQL注入特征
        /// </summary>
        private static bool _isSqlInjectionSafe(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            foreach (string pattern in sqlInjectionPatterns)            
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase)) return true;            
            return false;
        }
        /// <summary>
        /// 过滤SQL注入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string _isSqlClear(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            foreach (string pattern in sqlInjectionPatterns)
                input = Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
            return input;
        }
    }
}
