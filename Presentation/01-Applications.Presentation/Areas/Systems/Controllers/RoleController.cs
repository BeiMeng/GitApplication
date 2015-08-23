using System;
using System.Linq;
using System.Web.Mvc;
using Applications.Domains.Queries.Systems;
using Applications.Services.Dtos.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Services.Impl.Systems;
using Presentation.Base;
using Util.Webs.Mvc;

namespace Presentation.Areas.Systems.Controllers {
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : GridControllerBase<RoleDto, RoleQuery> {
        /// <summary>
        /// 初始化角色控制器
        /// </summary>
        /// <param name="service">角色服务</param>
        public RoleController( IRoleService service ) 
            : base( service ) {
            RoleService = service;
        }

        /// <summary>
        /// 角色服务
        /// </summary>
        public IRoleService RoleService { get; private set; }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="userId">用户编号</param>
        [HttpPost]
        [AjaxOnly]
        public ActionResult QueryByUser(RoleQuery query, Guid userId)
        {
            SetPage(query);
            var result = RoleService.Query(query, userId);
            return ToDataGridResult(ConvertQueryResult(result).ToList(), result.TotalCount);
        }
    }
}