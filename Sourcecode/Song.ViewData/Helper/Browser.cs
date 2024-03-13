using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Song.ViewData
{
    /// <summary>
    /// �������Ϣ
    /// </summary>
    public class Browser
    {

        private System.Web.HttpContext _context = null;
        public System.Web.HttpContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }
        public Browser(System.Web.HttpContext context)
        {
            this._context = context;
        }
        public Browser()
        {
            this._context = _context = System.Web.HttpContext.Current;
        }
        /// <summary>
        /// ��ǰ������Ƿ����ֻ������
        /// </summary>
        public bool IsMobile
        {
            get
            {
                try
                {
                    string u = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                    Regex b = new Regex(@"android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\/|plucker|pocket|psp|symbian|treo|up\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\\-(n|u)|c55\\/|capi|ccwa|cdm\\-|cell|chtm|cldc|cmd\\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\\-s|devi|dica|dmob|do(c|p)o|ds(12|\\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\\-|_)|g1 u|g560|gene|gf\\-5|g\\-mo|go(\\.w|od)|gr(ad|un)|haie|hcit|hd\\-(m|p|t)|hei\\-|hi(pt|ta)|hp( i|ip)|hs\\-c|ht(c(\\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\\-(20|go|ma)|i230|iac( |\\-|\\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\\/)|klon|kpt |kwc\\-|kyo(c|k)|le(no|xi)|lg( g|\\/(k|l|u)|50|54|e\\-|e\\/|\\-[a-w])|libw|lynx|m1\\-w|m3ga|m50\\/|ma(te|ui|xo)|mc(01|21|ca)|m\\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\\-2|po(ck|rt|se)|prox|psio|pt\\-g|qa\\-a|qc(07|12|21|32|60|\\-[2-7]|i\\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\\-|oo|p\\-)|sdk\\/|se(c(\\-|0|1)|47|mc|nd|ri)|sgh\\-|shar|sie(\\-|m)|sk\\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\\-|v\\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\\-|tdg\\-|tel(i|m)|tim\\-|t\\-mo|to(pl|sh)|ts(70|m\\-|m3|m5)|tx\\-9|up(\\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\\-|2|g)|yas\\-|your|zeto|zte\\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    if (string.IsNullOrWhiteSpace(u)) return false;
                    if (b.IsMatch(u)) return true;
                    if (u.Length >= 4 && v.IsMatch(u.Substring(0, 4))) return true;
                }
                catch
                {
                    return false;
                }
                return false;
            }
        }
        /// <summary>
        /// ��ǰ������Ƿ���΢�������
        /// </summary>
        public bool IsWeixin
        {
            get
            {
                string userAgent = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                if (userAgent.ToLower().Contains("micromessenger")) return true;
                return false;
            }
        }
        /// <summary>
        /// ǰ��������Ƿ���΢��С����
        /// </summary>
        public bool IsWeixinApp
        {
            get
            {
                string userAgent = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                if (userAgent.ToLower().Contains("miniprogram")) return true;
                return false;
            }
        }
        /// <summary>
        /// ǰ��������Ƿ�������Ӧ��
        /// </summary>
        public bool IsDestopApp
        {
            get
            {
                string userAgent = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"DeskApp\(.[^\)]*\)");
                if (b.IsMatch(userAgent)) return true;
                return false;
            }
        }
        /// <summary>
        /// �Ƿ���Apicloud�����APP��
        /// </summary>
        public bool IsAPICloud
        {
            get
            {
                string userAgent = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                if (userAgent.ToLower().Contains("apicloud")) return true;
                return false;
            }
        }

        /// <summary>
        /// ��ǰ������Ƿ�����ƻ���ֻ�
        /// </summary>
        public bool IsIPhone
        {
            get
            {
                string u = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"ip(hone|od)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (b.IsMatch(u))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// �ͻ���IP
        /// </summary>       
        public string IP
        {
            get
            {
                string result = _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //�����д���     
                    if (result.IndexOf(".") == -1)    //û��"."�϶��Ƿ�IPv4��ʽ     
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //��","�����ƶ������ȡ��һ������������IP��     
                            result = result.Replace(" ", "").Replace("\"", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //�ҵ����������ĵ�ַ     
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //������IP��ʽ     
                            return result;
                        else
                            result = null;    //�����е����� ��IP��ȡIP     
                    }
                }
                string IpAddress = (_context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : _context.Request.ServerVariables["REMOTE_ADDR"];
                if (null == result || result == String.Empty)
                    result = _context.Request.ServerVariables["REMOTE_ADDR"];
                if (result == null || result == String.Empty)
                    result = _context.Request.UserHostAddress;
                return result;
            }
        }
        /// <summary>
        /// վ��ķ�������
        /// </summary>
        /// <returns></returns>
        public string Domain
        {
            get
            {
                try
                {
                    return _context.Request.Url.Host.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// �Ƿ��Ǳ���IP
        /// </summary>
        public bool IsLocalIP
        {
            get
            {
                string ip = this.IP;
                if (ip == "127.0.0.1" || this.Domain == "127.0.0.1") return true;
                if (this.Domain.ToLower().Trim() == "localhost") return true;
                return false;
            }
        }
        /// <summary>
        /// �Ƿ�������IP
        /// </summary>
        public bool IsIntranetIP
        {
            get
            {
                if (IsLocalIP) return true;
                string ip = this.IP;
                if (ip.Substring(0, 3) == "10." || ip.Substring(0, 7) == "192.168" || ip.Substring(0, 7) == "172.16.")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>    
        /// �ж��Ƿ���IP��ַ��ʽ 0.0.0.0    
        /// </summary>    
        /// <param name="str1">���жϵ�IP��ַ</param>    
        /// <returns>true or false</returns>    
        private bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
        /// <summary>
        /// �ͻ��˵����������
        /// </summary>
        public string Type
        {
            get
            {
                HttpBrowserCapabilities bc = _context.Request.Browser;
                return bc.Type;
            }
        }
        /// <summary>
        /// �ͻ��˵������������
        /// </summary>
        public string Name
        {
            get
            {
                HttpBrowserCapabilities bc = _context.Request.Browser;
                return bc.Browser;
            }
        }
        /// <summary>
        /// �ͻ��˵�������İ汾��
        /// </summary>
        public string Version
        {
            get
            {
                HttpBrowserCapabilities bc = _context.Request.Browser;
                return bc.Version;
            }
        }
        /// <summary>
        /// ��ȡ�ͻ��˵Ĳ���ϵͳ����
        /// </summary>
        public string OS
        {
            get
            {
                string uagent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                if (uagent.Contains("Windows"))
                {
                    if (uagent.Contains("Windows NT 10")) return "Windows 10";
                    else if (uagent.Contains("Windows NT 11")) return "Windows 11";
                    else if (uagent.Contains("Windows NT 6.3")) return "Windows 8.1";
                    else if (uagent.Contains("Windows NT 6.2")) return "Windows 8";
                    else if (uagent.Contains("Windows NT 6.1")) return "Windows 7";
                    else if (uagent.Contains("Windows NT 6.0")) return "Windows Vista";
                    else if (uagent.Contains("Windows NT 5.1") || uagent.Contains("Windows NT 5.2")) return "Windows XP";
                    return "Windows";
                }
                else if (uagent.Contains("Mac")) return "Mac";
                else if (uagent.Contains("Android")) return "Android";
                else if (uagent.Contains("Linux")) return "Linux";
                else if (uagent.Contains("SunOS")) return "SunOS";
                else if (uagent.Contains("iOS")) return "iOS";
                return "";
            }
        }
        /// <summary>
        /// ��ȡ�ֻ�����ϵͳ��
        /// </summary>
        public string MobileOS
        {
            get
            {
                string u = _context.Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\/|plucker|pocket|psp|symbian|treo|up\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                //Match math = new Match();
                MatchCollection mc = b.Matches(u);
                if (mc.Count < 1) return OS;
                return mc[0].Value;
            }
        }
        /// <summary>
        /// ��ȡ�ֻ����루������ֻ����ʣ������Ǵ����ͨ�������ޣ��޷���ȡ
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                string mobile = "";
                if (_context.Request.ServerVariables["DEVICEID"] != null)
                {
                    mobile = _context.Request.ServerVariables["DEVICEID"].ToString();
                }
                if (_context.Request.ServerVariables["HTTP_X_UP_subno"] != null)
                {
                    mobile = _context.Request.ServerVariables["HTTP_X_UP_subno"].ToString();
                    mobile = mobile.Substring(3, 11);
                }
                if (_context.Request.ServerVariables["HTTP_X_NETWORK_INFO"] != null)
                {
                    mobile = _context.Request.ServerVariables["HTTP_X_NETWORK_INFO"].ToString();
                    //mobile = Right(mobile, mobile.Length - mobile.IndexOf(',')) ;
                    //mobile = Left(mobile, InStr(mobile, ",") - 1)   
                }
                if (_context.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"] != null)
                {
                    mobile = _context.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"].ToString();
                }
                return mobile;
            }
        }
        public override string ToString()
        {
            return this.Name + " " + this.Version;
        }
    }


}
