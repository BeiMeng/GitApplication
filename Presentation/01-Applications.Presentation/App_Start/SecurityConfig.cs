using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Presentation;

//注册
[assembly: OwinStartup( typeof( SecurityConfig ) )]

namespace Presentation {
    /// <summary>
    /// 安全配置
    /// </summary>
    public class SecurityConfig {
        /// <summary>
        /// 配置
        /// </summary>
        public void Configuration( IAppBuilder app ) {
            //使用cookie存储安全信息
            app.UseCookieAuthentication( new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString( "/Authentication/Login" )
            } );
        }
    }
}
