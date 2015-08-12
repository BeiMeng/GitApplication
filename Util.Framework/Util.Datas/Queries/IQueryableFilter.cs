using System.Linq;

namespace Util.Datas.Queries {
    /// <summary>
    /// 查询过滤器
    /// </summary>
    public interface IQueryableFilter<T> {
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <param name="queryable">查询对象</param>
        IQueryable<T> Filter( IQueryable<T> queryable );
    }
}
