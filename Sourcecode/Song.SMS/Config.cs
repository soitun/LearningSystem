using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Song.SMS
{
    public class Config
    {
        private static readonly Config _get = new Config();
        /// <summary>
        /// ��ȡ����(��������)
        /// </summary>
        public static Config Singleton
        {
            get { return _get; }
            
        }
        private static SmsItem[] smsItems;
        /// <summary>
        /// ����ƽ̨�б�
        /// </summary>
        public static SmsItem[] SmsItems
        {
            get { return Config.smsItems; }
        }
        private string _currentName;
        /// <summary>
        /// ��ǰ���õĶ���ƽ̨
        /// </summary>
        public string CurrentName
        {
            get { return _currentName; }
            set
            {
                _currentName = value;
                foreach (SmsItem item in smsItems)
                    item.IsCurrent = item.Mark.Equals(_currentName, StringComparison.OrdinalIgnoreCase);
            }
        }
        private Config()
        {            
            this.initialize();            
        }
        
        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private string _getValue(XmlNode node, string attr)
        {
            foreach (XmlAttribute abt in node.Attributes)
            {
                if (String.Equals(attr, abt.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return abt.Value;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// ��ʼ������ȡ������
        /// </summary>
        /// <returns></returns>
        private SmsItem[] initialize()
        {
            XmlNodeList list = WeiSha.Core.WebConfig.ChildNode("SMS").ChildNodes;
            List<SmsItem> smslist = new List<SmsItem>();
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode node = list[i];
                ////����ýӿڽ��ã�������
                //if (_getValue(node, "isUse") == "false")
                //    continue;
                SmsItem si = new SmsItem();
                //si.User = _getValue(node, "user");
                //si.Password = _getValue(node, "pw");
                string isuse = _getValue(node, "isUse");
                si.IsUse = "false".Equals(isuse, StringComparison.OrdinalIgnoreCase) ? false : true;
                si.Type = _getValue(node, "type");
                si.Mark = _getValue(node, "remarks");
                si.Name = _getValue(node, "name");
                if (string.IsNullOrWhiteSpace(si.Name))
                    si.Name = si.Mark;
                //�������ַ
                si.Domain = _getValue(node, "domain");
                if (!si.Domain.EndsWith("/")) si.Domain += "/";
                si.RegisterUrl = _getValue(node, "regurl");
                si.PayUrl = _getValue(node, "payurl");            
                smslist.Add(si);
            }
            smsItems= smslist.ToArray();
            return smsItems;
        }
        #region ������̬����
        /// <summary>
        /// ��ǰ���õĶ���ƽ̨
        /// </summary>
        public static SmsItem Current
        {
            get
            {
                SmsItem currentItem = null;
                foreach (SmsItem it in Config.SmsItems)
                {
                    if (it.IsCurrent)
                    {
                        currentItem = it;
                        break;
                    }
                }
                //���û�����ö���ƽ̨����Ĭ��ȡ��һ��
                if (currentItem == null)
                {
                    if (Config.SmsItems.Length > 0)
                        currentItem = Config.SmsItems[0];
                }
                return currentItem;
            }
        }
        /// <summary>
        /// ���õ�ǰ�Ķ���ƽ̨
        /// </summary>
        /// <param name="remarks"></param>
        public static void SetCurrent(string remak)
        {
            Config.Singleton.CurrentName = remak;          
        }
        /// <summary>
        /// ��ȡ�����ӿڶ���
        /// </summary>
        /// <param name="remak"></param>
        /// <returns></returns>
        public static SmsItem GetItem(string remak)
        {
            SmsItem smsitem = null;
            foreach (SmsItem item in smsItems)
            {
                if(item.Mark.Equals(remak, StringComparison.OrdinalIgnoreCase))
                {
                    smsitem = item;
                    break;
                }
            }
            return smsitem;
        }
        /// <summary>
        /// ���¶�ȡ���Žӿڵ�������
        /// </summary>
        public static void Fresh()
        {
            Config.Singleton.initialize();
        }
        #endregion
    }
}
