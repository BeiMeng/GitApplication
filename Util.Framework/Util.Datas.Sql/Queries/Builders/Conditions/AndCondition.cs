namespace Util.Datas.Sql.Queries.Builders.Conditions {
    /// <summary>
    /// And连接条件
    /// </summary>
    public class AndCondition : SqlCondition {
        /// <summary>
        /// 初始化Sql条件
        /// </summary>
        /// <param name="condition1">条件1</param>
        /// <param name="condition2">条件2</param>
        public AndCondition( string condition1, string condition2 ) {
            Condition = string.Format( "{0} And {1}", condition1, condition2 );
        }
    }
}
