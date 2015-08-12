using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Util;
using Util.ApplicationServices;
using Util.Domains.Repositories;
using Util.Security;
using Util.Webs;
using Util.Webs.EasyUi;
using Util.Webs.Mvc;

namespace Presentation.Base {
    /// <summary>
    /// 查询控制器
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    public abstract class QueryControllerBase<TDto, TQuery> : EasyUiControllerBase
        where TQuery : IPager
        where TDto : class ,IDto, new() {
        /// <summary>
        /// 初始化查询控制器
        /// </summary>
        /// <param name="service">服务</param>
        protected QueryControllerBase( IServiceBase<TDto, TQuery> service ) {
            Service = service;
        }

        /// <summary>
        /// 服务
        /// </summary>
        protected IServiceBase<TDto, TQuery> Service { get; private set; }

        /// <summary>
        /// 获取当前租户编号
        /// </summary>
        protected Guid? TenantId {
            get { return SecurityContext.Identity.TenantId.ToGuidOrNull(); }
        }

        /// <summary>
        /// 获取主界面
        /// </summary>
        public virtual ActionResult Index() {
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Query( TQuery query ) {
            SetPage( query );
            var result = Service.Query( query );
            return ToDataGridResult( ConvertQueryResult( result ).ToList(), result.TotalCount );
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="query">查询实体</param>
        protected void SetPage( IPager query ) {
            query.Page = GetPageIndex();
            query.PageSize = GetPageSize();
            query.Order = GetOrder();
        }

        /// <summary>
        /// 转换查询结果
        /// </summary>
        /// <param name="result">查询结果</param>
        protected virtual IEnumerable<dynamic> ConvertQueryResult( List<TDto> result ) {
            return result;
        }
    }
}
