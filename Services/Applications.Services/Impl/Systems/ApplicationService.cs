using System;
using System.Collections.Generic;
using System.Linq;
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
using Util.Datas;

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
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tenantId"></param>
        protected ApplicationDto ToDto(Application entity,Guid tenantId)
        {
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
        /// <summary>
        ///
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public PagerList<ApplicationDto> Query(ApplicationQuery param, Guid tenantId)
        {
            var query = GetQuery(param);
            var queryable = ApplicationRepository.Query(query);
            queryable = FileterQueryable(queryable);
            queryable = queryable.OrderBy(query.GetOrderBy()).Pager(query);
            //var list = queryable.ToPageList(query);
            // var result = new PagerList<ApplicationDto>( list.Page, list.PageSize, list.TotalCount, list.Order );
            //result.AddRange(list.Select(item => item.ToDto(tenantId)));

            ////result.AddRange( list.Select( ToDto(tenantId) ) );

            //return result;
            ////return .Convert(ToDto(tenantId));
            return queryable.ToPageList(query).Convert(item => item.ToDto(tenantId));
        }
    }
}