using System;
using System.Collections.Generic;
using Util.Domains;

namespace Applications.Domains.Services.Systems
{
    /// <summary>
    /// 应用程序管理器接口
    /// </summary>
    public interface IApplicationManager : IDomainService
    {
        /// <summary>
        /// 保存选中应用程序与租户关系信息
        /// </summary>
        /// <param name="applicationIds">选中应用程序ID集合</param>
        /// <param name="tenantId">租户ID</param>
        void SaveTenantInApplications(List<Guid> applicationIds, Guid tenantId);
    }
}