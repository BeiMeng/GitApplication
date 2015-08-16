using System;
using Util;
using Util.Domains;
using Util.Security;

namespace Applications.Domains.Core.Tenant.Model {
    /// <summary>
    /// 租户树型聚合根
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class TenantTreeAggregateRoot<TEntity> : TreeEntityBase<TEntity>, ITenant where TEntity : ITreeEntity<TEntity, Guid, Guid?>
    {
        /// <summary>
        /// 初始化字典
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="path">路径</param>
        /// <param name="level">级数</param>
        public TenantTreeAggregateRoot( Guid id, string path, int level )
            : base( id, path, level ) {
        }

        /// <summary>
        /// 获取当前租户编号
        /// </summary>
        protected Guid? GetTenantId() {
            return Conv.ToGuidOrNull( SecurityContext.Identity.TenantId );
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init() {
            base.Init();
            TenantId = GetTenantId();
        }

        public Guid? TenantId { get; set; }
    }
}
