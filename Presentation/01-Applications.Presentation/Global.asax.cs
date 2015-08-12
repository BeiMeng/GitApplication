using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Applications.Services.Configs.IocConfigs;
using Datas.Ef;
using Util;
using Util.ApplicationServices;
using Util.Datas.Ef;
using Util.Logs;
using Util.Logs.Log4;

namespace Presentation {
    /// <summary>
    /// 应用程序全局设置
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start() {
            WriteLog( "Application_Start准备启动" );
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
            Ioc.RegisterMvc( typeof( MvcApplication ).Assembly, new BeiDreamAppConfig() );
            EfUnitOfWork.Init( new BeiDreamAppUnitOfWork() );
            //EfUnitOfWork.Init( new MySqlApplicationUnitOfWork() );
            WriteLog( "Application_Start启动完成" );
        }

        /// <summary>
        /// 应用程序错误处理
        /// </summary>
        protected void Application_Error( object sender, EventArgs e ) {
            var lastError = Server.GetLastError();
            WriteLog( lastError );
            //Response.Redirect( @"~/error" );
            //Server.ClearError();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        private void WriteLog( string caption ) {
            ILog log = Log.GetContextLog( this );
            log.Caption.Add( caption );
            log.Debug();
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        private void WriteLog( Exception exception ) {
            ILog log = Log.GetContextLog( this );
            log.Caption.Add( "Global全局异常捕获" );
            log.Exception = exception;
            log.Error();
        }
    }
}