using System.ComponentModel;

namespace Util.Datas.Queries {
    /// <summary>
    /// 排序方向
    /// </summary>
    public enum OrderDirection {
        /// <summary>
        /// 升序
        /// </summary>
        [Description( "升序" )]
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        [Description( "降序" )]
        Desc
    }

    /// <summary>
    /// 排序方向枚举扩展
    /// </summary>
    public static class OrderDirectioneExtensions {
        /// <summary>
        /// 获取描述
        /// </summary>
        public static string Description( this OrderDirection? direction ) {
            return direction == null ? string.Empty : direction.Value.Description();
        }
    }
}
