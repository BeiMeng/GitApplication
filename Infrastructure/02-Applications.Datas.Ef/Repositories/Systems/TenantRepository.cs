using Applications.Domains.Models.Systems;
using Applications.Domains.Repositories.Systems;
using Infrastructure.Core;

namespace Datas.Ef.Repositories.Systems
{
    /// <summary>
    /// 租户仓储
    /// </summary>
    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository {
        /// <summary>
        /// 初始化租户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public TenantRepository(IBeiDreamAppUnitOfWork unitOfWork ) : base( unitOfWork ) {
        }
    }
}
