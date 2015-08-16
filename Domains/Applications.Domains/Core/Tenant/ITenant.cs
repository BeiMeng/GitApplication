using System;

namespace Applications.Domains.Core.Tenant
{
    /// <summary>
    /// 租户实现接口
    /// </summary>
    public interface ITenant<TKey>
    {
        /// <summary>
        /// 租户编号
        /// </summary>
        TKey TenantId { get; set; }
    }

    public interface ITenant : ITenant<Guid?>
    {

    }
}