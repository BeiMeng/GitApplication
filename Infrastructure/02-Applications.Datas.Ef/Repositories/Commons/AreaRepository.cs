using Applications.Domains.Models.Commons;
using Applications.Domains.Repositories.Commons;
using Infrastructure.Core;

namespace Datas.Ef.Repositories.Commons {
    /// <summary>
    /// 地区仓储
    /// </summary>
    public class AreaRepository : RepositoryBase<Area>, IAreaRepository {
        /// <summary>
        /// 初始化地区仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public AreaRepository(IBeiDreamAppUnitOfWork unitOfWork)
            : base( unitOfWork ) {
        }
    }
}
