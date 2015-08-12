using System.Web.Mvc;
using Infrastructure.Security;
using Util.Webs.EasyUi;

namespace Presentation.Controllers {
    /// <summary>
    /// 主界面控制器
    /// </summary>
    public class HomeController : EasyUiControllerBase {
        /// <summary>
        /// 初始化主界面控制器
        /// </summary>
        public HomeController() {
        }

        /// <summary>
        /// 获取主界面
        /// </summary>
        [RoleAuthorize(Ignore = true)]
        public virtual ActionResult Index() {
            return View();
        }
    }
}
