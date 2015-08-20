using System;
using System.Linq;
using System.Web.Mvc;
using Applications.Domains.Queries.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Services.Dtos.Systems;
using Presentation.Base;
using Util.Webs.Mvc;

namespace Presentation.Areas.Systems.Controllers {
    /// <summary>
    /// 应用程序控制器
    /// </summary>
    public class ApplicationController : FormControllerBase<ApplicationDto, ApplicationQuery> {
        /// <summary>
        /// 初始化应用程序控制器
        /// </summary>
        /// <param name="applicationService">应用程序服务</param>
        public ApplicationController( IApplicationService applicationService )
            : base( applicationService ) {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tenantId">查询对象</param>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Query(ApplicationQuery query, Guid tenantId)
        {
            SetPage(query);
            var result = Service.Query(query);
            return ToDataGridResult(ConvertQueryResult(result).ToList(), result.TotalCount);
        }
    }
}
