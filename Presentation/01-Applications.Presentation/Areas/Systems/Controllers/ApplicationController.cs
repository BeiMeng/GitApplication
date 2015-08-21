using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Applications.Domains.Queries.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Services.Dtos.Systems;
using Presentation.Base;
using Util;
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
            : base( applicationService )
        {
            ApplicationService = applicationService;
        }
        /// <summary>
        /// 租户服务
        /// </summary>
        protected IApplicationService ApplicationService { get; private set; }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="tenantId">租户编号</param>
        [HttpPost]
        [AjaxOnly]
        public ActionResult QueryByTenant(ApplicationQuery query, Guid tenantId)
        {
            SetPage(query);
            var result = ApplicationService.Query(query, tenantId);
            return ToDataGridResult(ConvertQueryResult(result).ToList(), result.TotalCount);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="ids">已选中的列表</param>
        /// <param name="tenantId"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormExceptionHandler]
        [AjaxOnly]
        public ActionResult SaveApplicationInTenant(string ids,Guid tenantId)
        {
                      
            return Ok(R.SaveSuccess);
        }
    }
}
