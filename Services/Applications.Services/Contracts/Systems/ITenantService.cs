using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 租户服务
    /// </summary>
    public interface ITenantService : ITreeBatchService<TenantDto, TenantQuery>
    {
    }
}