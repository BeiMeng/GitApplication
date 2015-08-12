using System;
using Applications.Domains.Models.Systems;
using Applications.Domains.Queries.Systems;
using Applications.Domains.Repositories.Systems;
using Util;
using Util.Domains.Repositories;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Infrastructure.Core;
using Util.ApplicationServices.Criterias;
using Util.Datas.Queries;

namespace Applications.Services.Impl.Systems {
    /// <summary>
    /// 租户服务
    /// </summary>
    public class TenantService : TreeBatchService<Tenant, TenantDto, TenantQuery>, ITenantService
    {
        
        #region 构造方法
        
        /// <summary>
        /// 初始化租户服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="tenantRepository">租户仓储</param>
        public TenantService( IBeiDreamAppUnitOfWork unitOfWork, ITenantRepository tenantRepository )
            : base( unitOfWork, tenantRepository ) {
            TenantRepository = tenantRepository;
        }
        
        #endregion

        #region 属性
        
        /// <summary>
        /// 租户仓储
        /// </summary>
        protected ITenantRepository TenantRepository { get; set; }
        
        #endregion
        
        #region 实体与Dto转换

        /// <summary>
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        protected override TenantDto ToDto( Tenant entity ) {
            return entity.ToDto();
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override Tenant ToEntity( TenantDto dto ) {
            return dto.ToEntity();
        }
        
        #endregion

        #region Create(创建新实体)

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <param name="parentId">父字典标识</param>
        public override TenantDto Create(string parentId)
        {
            var result = base.Create(parentId);
            result.Enabled = true;
            result.CreateTime = DateTime.Now;
            return result;
        }

        #endregion
        
        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">租户查询参数</param>
        protected override IQueryBase<Tenant> GetQuery( TenantQuery query ) {
            return new Query<Tenant>(query).Filter(new TreeEntityCriteria<Tenant>(query));
        }
        
        #endregion
    }
}