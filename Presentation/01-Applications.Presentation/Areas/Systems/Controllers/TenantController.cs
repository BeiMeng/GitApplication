using System;
using System.Web.Mvc;
using Applications.Domains.Queries.Systems;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Presentation.Base;
using Util.Webs.EasyUi.Trees;
using Util.Webs.Mvc;

namespace Presentation.Areas.Systems.Controllers {
    /// <summary>
    /// 租户控制器
    /// </summary>
    public class TenantController : TreeGridControllerBase<TenantDto, TenantQuery>
    {
        /// <summary>
        /// 初始化租户控制器
        /// </summary>
        /// <param name="service">租户服务</param>
        public TenantController( ITenantService service ) 
            : base( service ) {
            TenantService = service;
        }

        /// <summary>
        /// 租户服务
        /// </summary>
        protected ITenantService TenantService { get; private set; }
        /// <summary>
        /// 加载模式
        /// </summary>
        protected override LoadMode LoadMode
        {
            get { return LoadMode.Sync; }
        }
        /// <summary>
        /// 获取租户应用程序设置窗口
        /// </summary>
        /// <param name="id">租户ID</param>
        [AjaxOnly]
        public PartialViewResult EditApplication(Guid id)
        {
            ApplicationDto applicationDto = new ApplicationDto { TenantId = id };
            return PartialView("Parts/ApplicationInTenant", applicationDto);
        }
    }
}