using System.Linq;
using System.Web.Mvc;
using Applications.Domains.Queries.Commons;
using Applications.Services.Contracts.Commons;
using Applications.Services.Dtos.Commons;
using Presentation.Base;
using Util.Webs;
using Util.Webs.EasyUi.Trees;
using Util.Webs.Mvc;

namespace Presentation.Areas.Commons.Controllers {
    /// <summary>
    /// 字典控制器
    /// </summary>
    public class DicController : TreeGridControllerBase<DicDto, DicQuery> {
        /// <summary>
        /// 初始化字典控制器
        /// </summary>
        /// <param name="service">字典服务</param>
        public DicController( IDicService service )
            : base( service ) {
                Service = service;
        }

        /// <summary>
        /// 字典服务
        /// </summary>
        protected new IDicService Service { get; private set; }

        /// <summary>
        /// 加载模式
        /// </summary>
        protected override LoadMode LoadMode {
            get { return LoadMode.RootAsync; }
        }

        /// <summary>
        /// 字典控件
        /// </summary>
        /// <param name="code">字典编码</param>
        [AjaxOnly]
        public ActionResult DicControl( string code ) {
            var data = Service.Load( code,TenantId );
            var nodes = data.Select( t => new TreeNode { Id = t.Id, Text = t.Text, ParentId = t.ParentId, Level = t.Level, State = t.State } );
            return ToTreeResult( nodes );
        }
    }
}