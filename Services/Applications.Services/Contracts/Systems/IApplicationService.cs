using System;
using System.Collections.Generic;
using Applications.Domains.Models.Systems;
using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Util.Domains.Repositories;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public interface IApplicationService : IBatchService<ApplicationDto, ApplicationQuery>
    {
        PagerList<ApplicationDto> Query(ApplicationQuery query, Guid tenantId);
    }
}