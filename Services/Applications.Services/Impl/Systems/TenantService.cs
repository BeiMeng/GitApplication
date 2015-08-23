using System;
using System.Collections.Generic;
using Applications.Domains.Models.Systems;
using Applications.Domains.Queries.Systems;
using Applications.Domains.Repositories.Systems;
using Applications.Domains.Services.Systems;
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
        /// <param name="applicationManager">应用程序管理器</param>
        public TenantService( IBeiDreamAppUnitOfWork unitOfWork, ITenantRepository tenantRepository,IApplicationManager applicationManager )
            : base( unitOfWork, tenantRepository ) {
            TenantRepository = tenantRepository;
            ApplicationManager = applicationManager;
            }
        
        #endregion

        #region 属性
        
        /// <summary>
        /// 租户仓储
        /// </summary>
        protected ITenantRepository TenantRepository { get; set; }
        /// <summary>
        /// 应用程序管理器
        /// </summary>
        protected IApplicationManager ApplicationManager { get; set; }
        
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
            return new Query<Tenant>(query).Filter(new TreeEntityCriteria<Tenant>(query))
                .Filter(t => t.Code == query.Code)
                .Filter(t => t.Name.Contains(query.Name))
                .FilterDate(t => t.CreateTime, query.BeginCreateTime, query.EndCreateTime);
        }
        
        #endregion
        /// <summary>
        /// 设置租户拥有的应用程序
        /// </summary>
        /// <param name="ids">选择的应用程序ID集合</param>
        /// <param name="tenantId">租户ID</param>
        public void SaveTenantInApplications(List<Guid> ids, Guid tenantId)
        {
            UnitOfWork.Start();
            ApplicationManager.SaveTenantInApplications(ids, tenantId);
            UnitOfWork.Commit();
        }
    }
}