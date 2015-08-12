using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Util.Security.Webs {
    /// <summary>
    /// Web安全操作
    /// </summary>
    public class Helper {
        /// <summary>
        /// Form认证登入
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="isPersistent">是否持久保存到Cookie</param>
        public static void FormSignIn( string userId, bool isPersistent = false ) {
            FormsAuthentication.SetAuthCookie( userId, isPersistent );
        }

        /// <summary>
        /// Form认证退出
        /// </summary>
        public static void FormSignOut() {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 基于Asp.Net Identity的登入
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="isPersistent">是否持久保存到Cookie，即“记住密码”</param>
        public static void SignIn( string userId, bool isPersistent = false ) {
            var identity = CreateClaimsIdentity( userId );
            var authenticationManager = GetAuthenticationManager();
            authenticationManager.SignOut( DefaultAuthenticationTypes.ApplicationCookie );
            authenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, identity );
        }

        /// <summary>
        /// 创建声明标识
        /// </summary>
        private static ClaimsIdentity CreateClaimsIdentity( string userId ) {
            ClaimsIdentity identity = new ClaimsIdentity( DefaultAuthenticationTypes.ApplicationCookie );
            identity.AddClaim( new Claim( ClaimTypes.NameIdentifier, userId ) );
            identity.AddClaim( new Claim( ClaimTypes.Name, userId ) );
            identity.AddClaim( new Claim( "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string" ) );
            return identity;
        }

        /// <summary>
        /// 获取授权管理器
        /// </summary>
        private static IAuthenticationManager GetAuthenticationManager() {
            var owinContext = GetOwinContext();
            IAuthenticationManager authenticationManager = owinContext.Authentication;
            authenticationManager.CheckNull( "authenticationManager" );
            return authenticationManager;
        }

        /// <summary>
        /// 获取OWin上下文
        /// </summary>
        private static IOwinContext GetOwinContext() {
            var owinEnvironment = (IDictionary<string, object>)HttpContext.Current.Items["owin.Environment"];
            owinEnvironment.CheckNull( "owinEnvironment" );
            return new OwinContext( owinEnvironment );
        }

        /// <summary>
        /// 基于Asp.Net Identity的退出
        /// </summary>
        public static void SignOut() {
            var authenticationManager = GetAuthenticationManager();
            authenticationManager.SignOut( DefaultAuthenticationTypes.ApplicationCookie );
        }
    }
}
