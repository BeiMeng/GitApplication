using System.ComponentModel;
using Util;

namespace Applications.Domains.Enums {
    /// <summary>
    /// 租户状态
    /// </summary>
    public enum TenantState {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description( "待审核" )]
        Wait = 1,
        /// <summary>
        /// 已批准
        /// </summary>
        [Description( "已批准" )]
        Approve = 2
    }

    /// <summary>
    /// 租户状态枚举扩展
    /// </summary>
    public static class TenantStateExtensions {
        /// <summary>
        /// 获取描述
        /// </summary>
        public static string Description( this TenantState? state ) {
            return state == null ? string.Empty : state.Value.Description();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public static int? Value( this TenantState? state ) {
            if ( state == null )
                return null;
            return state.Value.Value();
        }
    }
}
