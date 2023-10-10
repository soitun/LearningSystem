using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using WeiSha.Core;

namespace Song.SMS.Object
{
    /// <summary>
    /// �������Ķ��ŷ�����
    /// </summary>
    public class DuanXinWang: ISMS
    {
        private SmsItem _current;
        public SmsItem Current
        {
            get { return _current; }
            set { _current = value; }
        }
        /// <summary>
        /// �û����˺�
        /// </summary>
        public string User
        {
            get { return _current.User; }
            set { _current.User = value; }
        }
        /// <summary>
        /// �û�������
        /// </summary>
        public string Password
        {
            get { return _current.Password; }
            set { _current.Password = value; }
        }
        public SmsState Send(string mobiles, string context)
        {
            return Send(mobiles, context, DateTime.Now);
        }

        public SmsState Send(string mobiles, string context, DateTime time)
        {
            //�����ʺ�������
            string account = Current.User;
            string pw = Current.Password;
            
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            
            //��ַ
            string url = "http://223.4.201.174/tx/";
            //����
            string postString = "user={0}&pass={1}&mobile={2}&content={3}&time={4}&encode=utf8";
            context = new WeiSha.Core.Param.Method.ConvertToAnyValue(context).UrlEncode;
            postString = string.Format(postString, account, pw, mobiles, context, time.ToString());
            byte[] postData = Encoding.UTF8.GetBytes(postString); 
            //WebClient webClient = new WebClient();
            ////��ȡPOST��ʽ����ӵ�header�������ΪGET��ʽ�Ļ���ȥ����仰����  
            //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            ////�õ������ַ�����������
            //byte[] responseData = webClient.UploadData(url, "POST", postData);
            //string result = Encoding.UTF8.GetString(responseData);

            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url+"?"+postString))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
             //����״̬
            SmsState state = new SmsState();
            state.Success = result == "100";
            //���ͽ����״̬
            string[] resultItem = new string[]{"100|���ͳɹ�","101|��֤ʧ��","102|���Ų���","103|����ʧ��","104|�Ƿ��ַ�",
               "105|���ݹ���","106|�������","107|Ƶ�ʹ���","108|�������ݿ�",
               "109|�˺Ŷ���","110|��ֹƵ����������","111|ϵͳ�ݶ�����",
               "112|���벻��ȷ","120|ϵͳ����"};
            foreach (string str in resultItem)
            {
                string s = str.Substring(0, str.IndexOf("|"));
                string e = str.Substring(str.IndexOf("|") + 1);
                if (result == s)
                {
                    state.Result = s;
                    state.Description = e;
                }
            }
            return state;
        }

        public int Query()
        {
            //�����ʺ�������
            string account = Current.User;
            string pw = Current.Password;
            return Query(account, pw);
        }
         /// <summary>
        /// ��ѯʣ��Ķ�������
        /// </summary>
        /// <param name="user">�˺�</param>
        /// <param name="pw">����</param>
        /// <returns></returns>
        public int Query(string user, string pw)
        {
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            //��ַ
            string url = "http://223.4.201.174/mm/?user={0}&pass={1}";
            url = string.Format(url, user, pw);
            //��ȡ��ʣ������
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
            if (result == "") throw new Exception("��ȡʧ�ܣ�");
            if (result.IndexOf("|") > -1)
            {
                string state = result.Substring(0, result.IndexOf("|"));
                //�����ȡ����
                if (state == "100")
                {
                    string num = result.Substring(result.IndexOf("-") + 1);
                    try
                    {
                        return System.Convert.ToInt32(num);
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
            return -1;
        }

        public string ReceiveSMS(DateTime from, string readflag)
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }
}
