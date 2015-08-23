using Applications.Domains.Models.Systems;
using Applications.Domains.Repositories.Systems;
using Infrastructure.Core;

namespace Datas.Ef.Repositories.Systems {
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository {
        /// <summary>
        /// 初始化角色仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public RoleRepository( IBeiDreamAppUnitOfWork unitOfWork ) : base( unitOfWork ) {
        }
    }
}
