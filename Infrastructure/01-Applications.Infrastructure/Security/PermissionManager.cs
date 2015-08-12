using Util.Security.Webs;

namespace Infrastructure.Security {
    /// <summary>
    /// 权限管理器
    /// </summary>
    public class PermissionManager : WebPermissionManager {
        /// <summary>
        /// 初始化权限管理器
        /// </summary>
        public PermissionManager()
            : base( new HttpContextAdapter(), new SecurityManager(), false ) {
        }
    }
}
