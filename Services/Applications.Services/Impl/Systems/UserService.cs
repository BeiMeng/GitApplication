using System;
using System.Collections.Generic;
using Applications.Domains.Models.Systems;
using Applications.Domains.Queries.Systems;
using Applications.Domains.Repositories.Systems;
using Applications.Domains.Services.Systems;
using Util;
using Util.Domains.Repositories;
using Util.Datas.Queries;
using Util.ApplicationServices;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Infrastructure.Core;

namespace Applications.Services.Impl.Systems {
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : BatchService<User, UserDto, UserQuery>, IUserService {
        
        #region 构造方法

        /// <summary>
        /// 初始化用户服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="roleManager">角色管理器</param>
        public UserService(IBeiDreamAppUnitOfWork unitOfWork, IUserRepository userRepository, IRoleManager roleManager)
            : base( unitOfWork, userRepository ) {
            UserRepository = userRepository;
            RoleManager = roleManager;
            }
        
        #endregion

        #region 属性
        
        /// <summary>
        /// 用户仓储
        /// </summary>
        protected IUserRepository UserRepository { get; set; }
        /// <summary>
        /// 角色管理器
        /// </summary>
        protected IRoleManager RoleManager { get; set; }
        
        #endregion
        
        #region 实体与Dto转换

        /// <summary>
        /// 转换为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        protected override UserDto ToDto( User entity ) {
            return entity.ToDto();
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override User ToEntity( UserDto dto ) {
            return dto.ToEntity();
        }
        
        #endregion
        
        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">用户查询参数</param>
        protected override IQueryBase<User> GetQuery( UserQuery query ) {
            return new Query<User>(query)
            .Filter(t => t.UserName.Contains(query.UserName))
            .Filter(t => t.Enabled == query.Enabled)
            .FilterDate(t => t.CreateTime, query.BeginCreateTime, query.EndCreateTime);
        }
        
        #endregion
        /// <summary>
        /// 设置用户拥有的角色
        /// </summary>
        /// <param name="ids">选择的角色ID集合</param>
        /// <param name="userId">用户ID</param>
        public void SaveUsersInRoles(List<Guid> ids, Guid userId)
        {
            UnitOfWork.Start();
            RoleManager.SaveUsersInRoles(ids, userId);
            UnitOfWork.Commit();
        }
    }
}