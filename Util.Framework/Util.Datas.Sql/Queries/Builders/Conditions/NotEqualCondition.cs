namespace Util.Datas.Sql.Queries.Builders.Conditions {
    /// <summary>
    /// Sql不相等条件
    /// </summary>
    public class NotEqualCondition : SqlCondition {
        /// <summary>
        /// 初始化Sql条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="prefix">参数前缀</param>
        public NotEqualCondition( string name, string prefix )
            : base( name, prefix ) {
        }

        /// <summary>
        /// 获取条件
        /// </summary>
        public override string GetCondition() {
            return string.Format( "{0}!={1}{2}", Name, Prefix,GetParamName( Name ) );
        }
    }
}
