using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;
using WeiSha.Core;

namespace Song.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //��ѯ��ʼ֮ǰ
            WeiSha.Data.Gateway.Default.RegisterLogger(new Song.ViewData.Helper.DatabaseLog());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //�����Զ���ģ������
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new WeiSha.Core.TemplateEngine());
            try
            {
                //�����е�ģ�����ã���ʼ��
                if (!WeiSha.Core.PlateOrganInfo.IsInitialization)
                    WeiSha.Core.Business.Do<ITemplate>().SetPlateOrganInfo();
                //��ʼ����ţ��ֱ����ֵ
                Business.Do<ILive>().Initialization();
                //�˺���Ϣ�������ڴ滺�棬���������ѯ
                //Song.ServiceImpls.AccountLogin.Buffer.Init();        
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex);
            }
        }
    }
}
