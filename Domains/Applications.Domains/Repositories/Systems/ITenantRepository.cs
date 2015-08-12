using Applications.Domains.Models.Systems;
using Util.Domains.Repositories;

namespace Applications.Domains.Repositories.Systems {
    /// <summary>
    /// 租户仓储
    /// </summary>
    public interface ITenantRepository : IRepository<Tenant> {
    }
}
