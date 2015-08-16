using System;
using System.ComponentModel.DataAnnotations;
using Util;
using Util.Domains;
using Util.Security;

namespace Applications.Domains.Core.Tenant.Model {
    /// <summary>
    /// 租户聚合根
    /// </summary>
    public class TenantAggregateRoot<TEntity> : AggregateRoot<TEntity>,ITenant where TEntity : IAggregateRoot<TEntity,Guid> {
        /// <summary>
        /// 初始化租户聚合根
        /// </summary>
        /// <param name="id">标识</param>
        public TenantAggregateRoot( Guid id )
            : base( id ) {
        }

        /// <summary>
        /// 获取当前租户编号
        /// </summary>
        protected Guid? GetTenantId() {
            return Conv.ToGuidOrNull( SecurityContext.Identity.TenantId );
        }

        /// <summary>
        /// 租户编号
        /// </summary>
        [Display( Name = "租户" )]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init() {
            base.Init();
            TenantId = GetTenantId();
        }
    }
}
