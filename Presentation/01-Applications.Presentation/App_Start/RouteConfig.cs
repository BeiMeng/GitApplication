using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation {
    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteConfig {
        /// <summary>
        /// 注册路由
        /// </summary>
        public static void RegisterRoutes( RouteCollection routes ) {
            IgnoreRoute( routes );
            RouteMaintenance( routes );
            RouteLogin( routes );
            RouteDefault( routes );
        }

        /// <summary>
        /// 忽略路由
        /// </summary>
        private static void IgnoreRoute( RouteCollection routes ) {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );
        }

        /// <summary>
        /// 注册维护页面
        /// </summary>
        private static void RouteMaintenance( RouteCollection routes ) {
            routes.MapRoute( "Maintenance", "maintenance", new { controller = "Error", action = "Maintenance" } );
        }

        /// <summary>
        /// 注册登录页面
        /// </summary>
        private static void RouteLogin( RouteCollection routes ) {
            routes.MapRoute( "Login", "Login", new { controller = "Home", action = "Login" } );
        }

        /// <summary>
        /// 注册默认路由，注意：默认路由必须在自定义路由之后
        /// </summary>
        private static void RouteDefault( RouteCollection routes ) {
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}