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
            identity.TenantId = "32ae412f-4291-4e0f-984f-c5967a836918";
            identity.Skin = siteManager.GetSkin();
            return identity;
        }
    }
}
