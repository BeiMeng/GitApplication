using Util.Datas.Sql.Queries;
using Util.Datas.Sql.Queries.Builders;
using Util.Datas.SqlServer.Queries.Builders;

namespace Util.Datas.SqlServer.Queries {
    /// <summary>
    /// SqlServer2012查询对象
    /// </summary>
    public class SqlServerQuery2012 : SqlQueryBase {
        /// <summary>
        /// 创建Sql生成器
        /// </summary>
        protected override ISqlBuilder CreateSqlBuilder() {
            return new SqlBuilder2012();
        }
    }
}
