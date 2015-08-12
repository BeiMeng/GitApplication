using System.Web.Mvc;

namespace Presentation.Controllers {
    /// <summary>
    /// 错误控制器
    /// </summary>
    public class ErrorController : Controller {
        /// <summary>
        /// 进入错误页面
        /// </summary>
        public ActionResult Index() {
            return View( "Error" );
        }

        /// <summary>
        /// 进入浏览器错误页面
        /// </summary>
        public ActionResult BrowserError() {
            return View( "BrowserError" );
        }

        /// <summary>
        /// 进入维护页面
        /// </summary>
        public ActionResult Maintenance() {
            return View( "Maintenance" );
        }
    }
}