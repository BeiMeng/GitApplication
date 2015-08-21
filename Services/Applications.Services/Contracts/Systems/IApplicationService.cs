using System;
using System.Collections.Generic;
using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public interface IApplicationService : IBatchService<ApplicationDto, ApplicationQuery>
    {
        List<ApplicationInTenantDto> GetApplicationInTenantDtos(ApplicationQuery query, Guid tenantId);
    }
}