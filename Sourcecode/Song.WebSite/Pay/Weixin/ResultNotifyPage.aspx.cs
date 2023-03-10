﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WeiSha.Core;
using Song.ServiceInterfaces;

namespace WxPayAPI
{
    /// <summary>
    /// 微信公众号支付的回调处理
    /// </summary>
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResultNotify resultNotify = new ResultNotify();
            //resultNotify.ProcessNotify();
            //获取结果
            WxPayData notifyData = resultNotify.GetNotifyData();
            string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
            Log.Info(this.GetType().ToString(), "商户流水号 : " + out_trade_no);
            if (!string.IsNullOrWhiteSpace(out_trade_no))
            {
                Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
                if (maccount != null)
                {
                    //付款方与收款方（商户id)
                    maccount.Ma_Buyer = notifyData.GetValue("attach").ToString();
                    maccount.Ma_Seller = notifyData.GetValue("mch_id").ToString();
                    Business.Do<IAccounts>().MoneyConfirm(maccount);

                    //刷新当前登录的学员信息
                    Song.Entities.Accounts acc= Business.Do<IAccounts>().AccountsSingle(maccount.Ac_ID);
                    Song.ViewData.LoginAccount.Fresh(acc);
                }
            }
            //notifyData.ToPrintStr();
        }       
    }
}