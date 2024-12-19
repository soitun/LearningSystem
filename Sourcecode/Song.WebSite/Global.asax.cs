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
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Song.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //ɾ��X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;

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
            ////����ͳ������,�ӳ�ִ��
            //WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData_Delay(10);
            ////������ʱ����
            //WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData_CronJob();

            //ִ�ж�ʱ����
            // ���� Quartz ������  
            StartScheduler().GetAwaiter().GetResult();

        }
        // ��ʱ����ĵ�����
        private static IScheduler _scheduler;
        private async Task StartScheduler()
        {
            // ����������  
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();


            // ������ҵ  
            IJobDetail job = JobBuilder.Create<ScheduledTaskJob>()
                .WithIdentity("ScheduledTaskJob", "group1")
                .Build();

            //��ʱ��ʱ�䣬CRON ���ʽ
            string cron = WeiSha.Core.App.Get["QueryDetail_Cron"].String;
            // ����������������ָ��ʱ��ִ��  
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithCronSchedule(cron) // CRON ���ʽ
                .Build();

            // ������ҵ  
            await _scheduler.ScheduleJob(job, trigger);

            await _scheduler.Start();
        }
        protected void Application_End(object sender, EventArgs e)
        {
            // ֹͣ���ͷ� Quartz ������  
            _scheduler?.Shutdown(waitForJobsToComplete: true);
        }
       
    }
    /// <summary>
    /// ��ʱ�����ִ����
    /// </summary>
    public class ScheduledTaskJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //ִ�з���          
            //����ͳ������
            WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData();
            await Task.CompletedTask;
        }
    }
}
