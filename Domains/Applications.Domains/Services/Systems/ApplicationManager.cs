using System;
using System.Collections.Generic;
using System.Linq;
using Applications.Domains.Repositories.Systems;
using Util;
using Util.Datas;
using Util.Domains;

namespace Applications.Domains.Services.Systems
{
    public class ApplicationManager : DomainServiceBase, IApplicationManager 
    {
        /// <summary>
        /// 初始化应用程序管理器
        /// </summary>
        /// <param name="applicationRepository">应用程序仓储</param>
        /// <param name="tenantRepository">租户仓储</param>
        public ApplicationManager(IApplicationRepository applicationRepository, ITenantRepository tenantRepository)
        {
            ApplicationRepository = applicationRepository;
            TenantRepository = tenantRepository;
        }
        /// <summary>
        ///应用程序仓储
        /// </summary>
        public IApplicationRepository ApplicationRepository { get; set; }

        /// <summary>
        ///租户仓储
        /// </summary>
        public ITenantRepository TenantRepository { get; set; }
        /// <summary>
        /// 保存选中应用程序与租户关系信息
        /// </summary>
        /// <param name="applicationIds">选中应用程序ID集合</param>
        /// <param name="tenantId">租户ID</param>
        public void SaveTenantInApplications(List<Guid> applicationIds, Guid tenantId)
        {
            applicationIds.CheckNull("applicationIds");
            tenantId.CheckNull("tenantId");
            var tenant = TenantRepository.Find(tenantId);
            if (tenant == null)
                throw new Warning("所设置的租户不存在");
            //删除此租户关联的应用程序
            tenant.Applications.Clear();
            //关联选中的应用程序
            foreach (var application in applicationIds.Select(id => ApplicationRepository.Single(p => p.Id == id)))
            {
                tenant.Applications.Add(application);
            }
        }
    }
}