using System;

namespace Util.Datas.Sql.Queries.Builders.Conditions {
    /// <summary>
    /// Sql条件
    /// </summary>
    public class SqlCondition : ISqlCondition {
        /// <summary>
        /// 初始化Sql条件
        /// </summary>
        protected SqlCondition() {
        }

        /// <summary>
        /// 初始化Sql条件
        /// </summary>
        /// <param name="condition">条件</param>
        protected SqlCondition( string condition ) {
            Condition = condition;
        }

        /// <summary>
        /// 初始化Sql条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="prefix">参数前缀</param>
        protected SqlCondition( string name, string prefix ) {
            Name = name;
            Prefix = prefix;
        }

        /// <summary>
        /// Sql条件
        /// </summary>
        protected string Condition { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// 参数前缀
        /// </summary>
        protected string Prefix { get; private set; }

        /// <summary>
        /// 获取条件
        /// </summary>
        public virtual string GetCondition() {
            return Condition;
        }

        /// <summary>
        /// 创建Sql条件
        /// </summary>
        /// <param name="condition">条件</param>
        public static ISqlCondition Create( string condition ) {
            return new SqlCondition( condition );
        }

        /// <summary>
        /// 创建Sql条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="prefix">参数前缀，范例@</param>
        /// <param name="operator">操作符</param>
        public static ISqlCondition Create( string name, string prefix, Operator @operator ) {
            switch ( @operator ) {
                case Operator.Equal:
                    return new EqualCondition( name ,prefix );
                case Operator.NotEqual:
                    return new NotEqualCondition( name, prefix );
                case Operator.Greater:
                    return new GreaterCondition( name, prefix );
                case Operator.Less:
                    return new LessCondition( name, prefix );
                case Operator.GreaterEqual:
                    return new GreaterEqualCondition( name, prefix );
                case Operator.LessEqual:
                    return new LessEqualCondition( name, prefix );
                case Operator.Contains:
                    return new LikeCondition( name, prefix );
                case Operator.Starts:
                    return new LikeCondition( name, prefix );
                case Operator.Ends:
                    return new LikeCondition( name, prefix );
            }
            throw new NotImplementedException( "该运算符未实现" );
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        /// <param name="column">列名</param>
        public static string GetParamName( string column ) {
            return column.Replace( ".", "_" ).Replace( "[","" ).Replace( "]","" );
        }
    }
}
