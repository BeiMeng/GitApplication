using Applications.Domains.Queries.Systems;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Presentation.Base;

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
    }
}