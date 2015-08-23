using System;
using System.Collections.Generic;
using Util.Domains;

namespace Applications.Domains.Services.Systems
{
    /// <summary>
    /// 角色管理器接口
    /// </summary>
    public interface IRoleManager : IDomainService
    {
        /// <summary>
        /// 保存选中角色与用户关系信息
        /// </summary>
        /// <param name="roleIds">选中角色ID集合</param>
        /// <param name="userId">用户ID</param>
        void SaveUsersInRoles(List<Guid> roleIds, Guid userId);
    }
}