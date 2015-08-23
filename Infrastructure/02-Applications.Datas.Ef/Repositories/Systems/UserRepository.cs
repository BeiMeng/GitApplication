using Applications.Domains.Models.Systems;
using Applications.Domains.Repositories.Systems;
using Infrastructure.Core;

namespace Datas.Ef.Repositories.Systems
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// 初始化用户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public UserRepository(IBeiDreamAppUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}