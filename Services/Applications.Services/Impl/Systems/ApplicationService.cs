using Applications.Domains.Queries.Systems;
using Applications.Domains.Repositories.Systems;
using Util;
using Util.Domains.Repositories;
using Util.Datas.Queries;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Domains.Models.Systems;
using Infrastructure.Core;

namespace Applications.Services.Impl.Systems {
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public class ApplicationService : BatchService<Application, ApplicationDto, ApplicationQuery>, IApplicationService {
        
        #region 构造方法
        
        /// <summary>
        /// 初始化应用程序服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="applicationRepository">应用程序仓储</param>
        public ApplicationService(IBeiDreamAppUnitOfWork unitOfWork, IApplicationRepository applicationRepository )
            : base( unitOfWork, applicationRepository ) {
            ApplicationRepository = applicationRepository;
        }
        
        #endregion

        #region 属性
        
        /// <summary>
        /// 应用程序仓储
        /// </summary>
        protected IApplicationRepository ApplicationRepository { get; set; }
        
        #endregion
        
        #region 实体与Dto转换

        /// <summary>
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        protected override ApplicationDto ToDto( Application entity ) {
            return entity.ToDto();
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override Application ToEntity( ApplicationDto dto ) {
            return dto.ToEntity();
        }
        
        #endregion
        
        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">应用程序查询参数</param>
        protected override IQueryBase<Application> GetQuery( ApplicationQuery query ) {
            return new Query<Application>( query );
        }
        
        #endregion
    }
}