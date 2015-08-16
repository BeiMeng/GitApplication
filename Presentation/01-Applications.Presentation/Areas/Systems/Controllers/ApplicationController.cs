using Applications.Domains.Queries.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Services.Dtos.Systems;
using Presentation.Base;

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
    }
}
