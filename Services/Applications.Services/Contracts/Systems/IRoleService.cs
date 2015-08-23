using System;
using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Util.Domains.Repositories;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService : IBatchService<RoleDto, RoleQuery>
    {
        /// <summary>
        ///查询角色数据
        /// </summary>
        /// <param name="param">查询对象</param>
        /// <param name="userId">用户ID,与此用户ID关联的角色checked属性设置为true</param>
        /// <returns></returns>
        PagerList<RoleDto> Query(RoleQuery param, Guid userId);
    }
}