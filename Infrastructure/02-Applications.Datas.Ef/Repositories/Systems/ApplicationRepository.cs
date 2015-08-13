using Applications.Domains.Repositories.Systems;
using Infrastructure.Core;
using Applications.Domains.Models.Systems;

namespace Datas.Ef.Repositories.Systems
{
    /// <summary>
    /// 应用程序仓储
    /// </summary>
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        /// <summary>
        /// 初始化租户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public ApplicationRepository(IBeiDreamAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}