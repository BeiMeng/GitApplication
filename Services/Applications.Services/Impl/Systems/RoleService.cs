using System;
using Applications.Domains.Models.Systems;
using Applications.Domains.Queries.Systems;
using Applications.Domains.Repositories.Systems;
using Util;
using Util.Domains.Repositories;
using Util.Datas.Queries;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Infrastructure.Core;
using Util.Datas;

namespace Applications.Services.Impl.Systems {
    /// <summary>
    /// 角色服务
    /// </summary>
    public class RoleService : BatchService<Role, RoleDto, RoleQuery>, IRoleService {
        
        #region 构造方法
        
        /// <summary>
        /// 初始化角色服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="roleRepository">角色仓储</param>
        public RoleService( IBeiDreamAppUnitOfWork unitOfWork, IRoleRepository roleRepository )
            : base( unitOfWork, roleRepository ) {
            RoleRepository = roleRepository;
        }
        
        #endregion

        #region 属性
        
        /// <summary>
        /// 角色仓储
        /// </summary>
        protected IRoleRepository RoleRepository { get; set; }
        
        #endregion
        
        #region 实体与Dto转换

        /// <summary>
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        protected override RoleDto ToDto( Role entity ) {
            return entity.ToDto();
        }
        /// <summary>
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户编号</param>
        protected RoleDto ToDto(Role entity, Guid userId)
        {
            return entity.ToDto();
        }
        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override Role ToEntity( RoleDto dto ) {
            return dto.ToEntity();
        }
        
        #endregion
        
        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">角色查询参数</param>
        protected override IQueryBase<Role> GetQuery( RoleQuery query ) {
            return new Query<Role>(query)
            .Filter(t => t.Code == query.Code)
            .Filter(t => t.Name.Contains(query.Name))
            .Filter(t => t.Enabled == query.Enabled)
            .FilterDate(t => t.CreateTime, query.BeginCreateTime, query.EndCreateTime);
        }
        
        #endregion
        /// <summary>
        ///查询角色数据
        /// </summary>
        /// <param name="param">查询对象</param>
        /// <param name="userId">用户ID,与此用户ID关联的角色checked属性设置为true</param>
        /// <returns></returns>
        public PagerList<RoleDto> Query(RoleQuery param, Guid userId)
        {
            var query = new Query<Role>(param);
            var queryable = RoleRepository.Query(query);
            queryable = FileterQueryable(queryable);
            queryable = queryable.OrderBy(query.GetOrderBy()).Pager(query);
            return queryable.ToPageList(query).Convert(item => item.ToDto(userId));
        }
    }
}