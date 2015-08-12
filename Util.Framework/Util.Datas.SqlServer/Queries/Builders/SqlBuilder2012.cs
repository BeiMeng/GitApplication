using System.Text;

namespace Util.Datas.SqlServer.Queries.Builders {
    /// <summary>
    /// SqlServer2012 Sql生成器
    /// </summary>
    public class SqlBuilder2012 : SqlBuilder{
        /// <summary>
        /// 创建分页Sql
        /// </summary>
        protected override void CreatePagerSql( StringBuilder result ) {
            result.Append( GetSelect() );
            AppendSqlBody( result );
            result.Append( GetOrderBy() );
            result.AppendFormat( "Offset {0} Rows Fetch Next {1} Rows Only", Pager.GetSkipCount(), Pager.PageSize );
        }
    }
}
