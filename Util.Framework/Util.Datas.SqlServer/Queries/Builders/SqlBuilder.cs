using System.Text;
using Util.Datas.Sql.Queries.Builders;

namespace Util.Datas.SqlServer.Queries.Builders {
    /// <summary>
    /// SqlServer Sql生成器
    /// </summary>
    public class SqlBuilder : SqlBuilderBase {
        /// <summary>
        /// 获取参数前缀
        /// </summary>
        protected override string GetParamPrefix() {
            return "@";
        }

        /// <summary>
        /// 获取Sql
        /// </summary>
        protected override string GetSql( StringBuilder result ) {
            if ( Pager == null )
                CreateNoPagerSql( result );
            else 
                CreatePagerSql( result );
            return result.ToString();
        }

        /// <summary>
        /// 创建不分页Sql
        /// </summary>
        private void CreateNoPagerSql(StringBuilder result ) {
            result.Append( GetSelect() );
            AppendSqlBody( result );
            result.Append( GetOrderBy() );
        }

        /// <summary>
        /// 添加Sql共享子句
        /// </summary>
        protected void AppendSqlBody( StringBuilder result ) {
            result.Append( GetFrom() );
            result.Append( GetWhere() );
            result.Append( GetGroupBy() );
        }

        /// <summary>
        /// 创建分页Sql
        /// </summary>
        protected virtual void CreatePagerSql( StringBuilder result ) {
            result.Append( "With Temp As ( " );
            result.AppendFormat( "{0}, Row_Number() Over( {1}) as SortNumber ", GetSelect(), GetOrderBy() );
            AppendSqlBody( result );
            result.Append( ") " );
            result.AppendFormat( "Select * From Temp Where SortNumber Between {0} and {1}", Pager.GetStartNumber(), Pager.GetEndNumber() );
        }
    }
}
