using System;
using System.Collections.Generic;
using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 租户服务
    /// </summary>
    public interface ITenantService : ITreeBatchService<TenantDto, TenantQuery>
    {
        /// <summary>
        /// 设置租户拥有的应用程序
        /// </summary>
        /// <param name="ids">选择的应用程序ID集合</param>
        /// <param name="tenantId">租户ID</param>
        void SaveTenantInApplications(List<Guid> ids, Guid tenantId);
    }
}