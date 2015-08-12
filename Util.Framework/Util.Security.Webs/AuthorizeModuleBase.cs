using System;
using System.Security.Principal;
using System.Web;

namespace Util.Security.Webs {
    /// <summary>
    /// 授权模块
    /// </summary>
    public abstract class AuthorizeModuleBase : IHttpModule {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init( HttpApplication context ) {
            context.PostAuthenticateRequest += Authenticate;
        }

        /// <summary>
        /// 授权
        /// </summary>
        public void Authenticate( object sender, EventArgs e ) {
            var application = sender as HttpApplication;
            if ( application == null )
                return;
            if ( !AuthenticateBefore( application ) )
                return;
            if ( IsResource( application ) )
                return;
            if ( !IsAuthenticated( application ) ) {
                HttpContext.Current.User = new UnauthenticatedPrincipal();
                return;
            }
            HttpContext.Current.User = GetPrincipal( application );
            AuthenticateAfter();
        }

        /// <summary>
        /// 授权前操作
        /// </summary>
        protected virtual bool AuthenticateBefore( HttpApplication application ) {
            return true;
        }

        /// <summary>
        /// 是否静态资源
        /// </summary>
        private bool IsResource( HttpApplication application ) {
            string extension = application.Request.CurrentExecutionFilePathExtension;
            switch ( extension.ToStr().ToLower() ) {
                case ".js":
                    return true;
                case ".css":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        private bool IsAuthenticated( HttpApplication application ) {
            if ( application.User == null )
                return false;
            if ( !application.User.Identity.IsAuthenticated )
                return false;
            return true;
        }

        /// <summary>
        /// 获取安全主体
        /// </summary>
        private IPrincipal GetPrincipal( HttpApplication application ) {
            return new Principal( GetIdentity( application.User.Identity.Name ) );
        }

        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="userId">用户编号</param>
        protected abstract IIdentity GetIdentity( string userId );

        /// <summary>
        /// 授权后操作
        /// </summary>
        protected virtual void AuthenticateAfter() {
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
        }
    }
}