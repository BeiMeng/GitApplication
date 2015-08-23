using System;
using System.Web.Mvc;
using Applications.Domains.Queries.Systems;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Presentation.Base;
using Util;
using Util.Webs.Mvc;

namespace Presentation.Areas.Systems.Controllers {
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : GridControllerBase<UserDto, UserQuery>
    {
        /// <summary>
        /// 初始化用户控制器
        /// </summary>
        /// <param name="service">用户服务</param>
        public UserController( IUserService service ) 
            : base( service ) {
            UserService = service;
        }

        /// <summary>
        /// 用户服务
        /// </summary>
        protected IUserService UserService { get; private set; }
        /// <summary>
        /// 获取角色设置窗口
        /// </summary>
        /// <param name="id">用户ID</param>
        [AjaxOnly]
        public PartialViewResult EditRoles(Guid id)
        {
            RoleDto roleDto = new RoleDto { UserId = id };
            return PartialView("Parts/UsersInRoles", roleDto);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="ids">选中的角色ID集合</param>
        /// <param name="userId">租户ID</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormExceptionHandler]
        [AjaxOnly]
        public ActionResult SaveUsersInRoles(string ids, Guid userId)
        {
            UserService.SaveUsersInRoles(ids.ToGuidList(), userId);
            return Ok(R.SaveSuccess);
        }
    }
}