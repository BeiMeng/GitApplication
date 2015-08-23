using System;
using System.Collections.Generic;
using Applications.Domains.Queries.Systems;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;

namespace Applications.Services.Contracts.Systems {
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService : IBatchService<UserDto, UserQuery> {
        /// <summary>
        /// 设置用户拥有的角色
        /// </summary>
        /// <param name="ids">选择的角色ID集合</param>
        /// <param name="userId">用户ID</param>
        void SaveUsersInRoles(List<Guid> ids, Guid userId);
    }
}