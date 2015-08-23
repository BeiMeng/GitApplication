using System;
using System.Collections.Generic;
using System.Linq;
using Applications.Domains.Repositories.Systems;
using Util;
using Util.Domains;

namespace Applications.Domains.Services.Systems
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class RoleManager : DomainServiceBase, IRoleManager
    {
        /// <summary>
        /// 初始化角色管理器
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="roleRepository">角色仓储</param>
        public RoleManager(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
        }
        /// <summary>
        ///用户仓储
        /// </summary>
        public IUserRepository UserRepository { get; set; }

        /// <summary>
        ///角色仓储
        /// </summary>
        public IRoleRepository RoleRepository { get; set; }
        /// <summary>
        /// 保存选中角色与用户关系信息
        /// </summary>
        /// <param name="roleIds">选中角色ID集合</param>
        /// <param name="userId">用户ID</param>
        public void SaveUsersInRoles(List<Guid> roleIds, Guid userId)
        {
            roleIds.CheckNull("roleIds");
            userId.CheckNull("userId");
            var user = UserRepository.Find(userId);
            if (user == null)
                throw new Warning("所设置的用户不存在");
            //删除此用户关联的角色
            user.Roles.Clear();
            //关联选中的角色
            foreach (var role in roleIds.Select(id => RoleRepository.Single(p => p.Id == id)))
            {
                user.Roles.Add(role);
            }
        }
    }
}