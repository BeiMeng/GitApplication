using System.Linq;
using System.Web.Mvc;
using Applications.Services.Contracts.Identity;
using Infrastructure.Security;
using Util;
using Util.Webs.Controls.ValidateCodes;
using Util.Webs.EasyUi;
using Util.Webs.EasyUi.Results;
using Util.Webs.EasyUi.Trees;

namespace Presentation.Controllers {
    /// <summary>
    /// 安全授权控制器
    /// </summary>
    public class AuthenticationController : EasyUiControllerBase {
        /// <summary>
        /// 初始化安全授权控制器
        /// </summary>
        /// <param name="identityService">系统服务</param>
        /// <param name="validateCode">验证码</param>
        public AuthenticationController(IIdentityService identityService, IValidateCode validateCode)
        {
            IdentityService = identityService;
            ValidateCode = validateCode;
        }

        /// <summary>
        /// 系统服务
        /// </summary>
        public IIdentityService IdentityService { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public IValidateCode ValidateCode { get; set; }

        /// <summary>
        /// 生成验证码
        /// </summary>
        [AllowAnonymous]
        public void GenerateValidateCode() {
            ValidateCode.IsGenereateDisturbLine = true;
            ValidateCode.Width = 120;
            ValidateCode.Height = 35;
            ValidateCode.FontSize = 20;
            ValidateCode.Generate();
        }

        /// <summary>
        /// 获取登录界面
        /// </summary>
        [AllowAnonymous]
        public ActionResult Login() {
            return View( "Login" );
        }

        /// <summary>
        /// 获取登录界面
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login( string userName) {
            Util.Security.Webs.Helper.SignIn( "a",true );
            return Ok();
        }

        [RoleAuthorize(Ignore = true)]
        public ActionResult GetMeanus() {
            return Content( GetTree() );
            //var modules = SecurityService.GetModules();
            //var nodes = modules.Select( t => new TreeNode() { Id = t.Id.ToStr(), ParentId = t.ParentId.ToStr(), Text = t.Name, Icon = t.SmallIcon, Attributes = new { url = t.GetUrl() } } );
            //return ToTreeResult( nodes );
        }

        public string GetTree() {
            var node = new TreeNode { Id = "1", Text = "系统管理" };
            var node1 = new TreeNode { Id = "2", ParentId = "1", Text = "应用程序管理", Attributes = new { url = "/systems/application" } };
            var node2 = new TreeNode { Id = "3", ParentId = "1", Text = "租户管理", Attributes = new { url = "/systems/tenant" } };
            var node3 = new TreeNode { Id = "4", ParentId = "1", Text = "字典管理", Attributes = new { url = "/commons/dic" } };
            var node4 = new TreeNode { Id = "5", ParentId = "1", Text = "地区管理", Attributes = new { url = "/commons/area" } };
            var node123 = new TreeNode { Id = "123", ParentId = "1", Text = "系统配置管理2", Attributes = new { url = "/configs/systemconfig" } };
            var node5 = new TreeNode { Id = "6", ParentId = "1", Text = "用户管理", Attributes = new { url = "/systems/user" } };
            var node6 = new TreeNode { Id = "7", ParentId = "1", Text = "角色管理", Attributes = new { url = "/systems/role" } };
            var node7 = new TreeNode { Id = "8", ParentId = "1", Text = "资源管理", Attributes = new { url = "/systems/resource" } };
            var node8 = new TreeNode { Id = "9", ParentId = "1", Text = "图标管理", Attributes = new { url = "/commons/icon" } };
            var node9 = new TreeNode { Id = "10", ParentId = "1", Text = "站点管理", Attributes = new { url = "/systems/site" } };
            var node20 = new TreeNode { Id = "20", ParentId = "1", Text = "测试", Attributes = new { url = "/Desktop" } };
            return new TreeResult( new[] { node, node1, node123, node2, node3, node4, node5, node6, node7, node8, node9, node20 } ).ToString();
        }
    }
}