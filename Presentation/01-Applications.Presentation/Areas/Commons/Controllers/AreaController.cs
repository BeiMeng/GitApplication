using System.Linq;
using System.Web.Mvc;
using Applications.Domains.Queries.Commons;
using Applications.Services.Contracts.Commons;
using Applications.Services.Dtos.Commons;
using Presentation.Base;
using Util;
using Util.Webs.EasyUi.Forms.Comboxs;
using Util.Webs.Mvc;

namespace Presentation.Areas.Commons.Controllers {
    /// <summary>
    /// 地区控制器
    /// </summary>
    public class AreaController : TreeGridControllerBase<AreaDto, AreaQuery> {
        /// <summary>
        /// 初始化地区控制器
        /// </summary>
        /// <param name="service">地区服务</param>
        public AreaController( IAreaService service )
            : base( service ) {
            Service = service;
        }

        /// <summary>
        /// 地区服务
        /// </summary>
        protected new IAreaService Service { get; private set; }

        /// <summary>
        /// 加载地区
        /// </summary>
        [AjaxOnly]
        public ActionResult AreaControl( string id ) {
            var result = Service.Load( id.ToGuidOrNull() );
            var items = result.Select( t => new ComboxItem( t.Text, t.Id ) );
            return ToComboxResult( items );
        }
    }
}