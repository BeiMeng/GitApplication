using System.Web.Mvc;
using Presentation.Base;
using Util.Webs.EasyUi;

namespace Presentation.Controllers {
    /// <summary>
    /// 桌面控制器
    /// </summary>
    public class DesktopController : EasyUiControllerBase {
        /// <summary>
        /// 获取主界面
        /// </summary>
        public virtual ActionResult Index() {
            return View();
        }
    }
}
