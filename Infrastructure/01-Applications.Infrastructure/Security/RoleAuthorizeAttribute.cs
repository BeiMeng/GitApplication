using System;
using System.Web;
using Util.Security;
using Util.Security.Webs;

namespace Infrastructure.Security {
    /// <summary>
    /// 基于角色授权
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
    public class RoleAuthorizeAttribute : AuthorizeAttributeBase {
        /// <summary>
        /// 创建权限管理器
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        protected override IPermissionManager CreatePermissionManager( HttpContextBase httpContext ) {
            return new WebPermissionManager( new HttpContextBaseAdapter( httpContext ), new SecurityManager(), Ignore );
        }
    }
}
