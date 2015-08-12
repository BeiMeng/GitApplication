using System;
using System.Web;
using System.Web.Mvc;

namespace Util.Security.Webs {
    /// <summary>
    /// 授权
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
    public abstract class AuthorizeAttributeBase : AuthorizeAttribute {
        /// <summary>
        /// 忽视角色检查，但仍然验证登录，如果需要完全忽视权限，应使用[AllowAnonymous]特性
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 资源标识
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        protected sealed override bool AuthorizeCore( HttpContextBase httpContext ) {
            return CreatePermissionManager( httpContext ).HasPermission( Uri );
        }

        /// <summary>
        /// 创建权限管理器
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        protected abstract IPermissionManager CreatePermissionManager( HttpContextBase httpContext );
    }
}
