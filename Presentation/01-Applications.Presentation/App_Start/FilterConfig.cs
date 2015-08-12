using System.Web.Mvc;
using Util.Webs.Mvc;

namespace Presentation {
    /// <summary>
    /// 过滤器配置
    /// </summary>
    public class FilterConfig {
        /// <summary>
        /// 注册全局过滤器
        /// </summary>
        public static void RegisterGlobalFilters( GlobalFilterCollection filters ) {
            filters.Add( new TraceLogAttribute() );
            filters.Add( new ErrorLogAttribute() );
            //filters.Add( new RoleAuthorizeAttribute() );
        }
    }
}
