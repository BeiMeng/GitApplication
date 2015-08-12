using System.Security.Principal;
using Applications.Domains.Services.Systems;
using Util.ApplicationServices;
using Util.Security;
using Util.Security.Webs;

namespace Infrastructure.Security {
    /// <summary>
    /// 授权模块
    /// </summary>
    public class AuthorizeModule : AuthorizeModuleBase {
        /// <summary>
        /// 获取身份标识
        /// </summary>
        /// <param name="userId">用户编号</param>
        protected override IIdentity GetIdentity( string userId ) {
            var identity = new Identity( true, userId );
            var siteManager = Ioc.Create<ISiteManager>();
            identity.TenantId = "CC8C48E7-CE1F-4460-8634-C1E798263EDA";
            identity.Skin = siteManager.GetSkin();
            return identity;
        }
    }
}
