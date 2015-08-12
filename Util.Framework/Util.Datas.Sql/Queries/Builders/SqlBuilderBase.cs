using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Datas.Sql.Queries.Builders.Conditions;
using Util.Domains.Repositories;

namespace Util.Datas.Sql.Queries.Builders {
    /// <summary>
    /// Sql生成器
    /// </summary>
    public abstract class SqlBuilderBase : ISqlBuilder {

        #region 构造方法

        /// <summary>
        /// 初始化Sql生成器
        /// </summary>
        protected SqlBuilderBase() {
            _params = new Dictionary<string, object>();
        }

        #endregion

        #region 字段属性

        /// <summary>
        /// 查询条件
        /// </summary>
        private ISqlCondition _condition;

        /// <summary>
        /// 参数列表
        /// </summary>
        private readonly Dictionary<string, object> _params;

        /// <summary>
        /// 表名
        /// </summary>
        protected string Table { get; private set; }

        /// <summary>
        /// 列名
        /// </summary>
        protected string Columns { get; private set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        protected string Order { get; private set; }

        /// <summary>
        /// 分组字段
        /// </summary>
        protected string Group { get; private set; }

        /// <summary>
        /// 分组条件
        /// </summary>
        protected string Having { get; private set; }

        /// <summary>
        /// 分页对象
        /// </summary>
        protected IPager Pager { get; set; }

        #endregion

        #region 基础设置

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        public ISqlBuilder From( string table ) {
            Table = table;
            return this;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        public ISqlBuilder Select( string columns ) {
            Columns = columns;
            return this;
        }

        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">分组列名</param>
        /// <param name="having">分组条件</param>
        public ISqlBuilder GroupBy( string group, string having = "" ) {
            Group = group;
            Having = having;
            return this;
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="order">排序条件</param>
        public ISqlBuilder OrderBy( string order ) {
            Order = order;
            return this;
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数</param> 
        public ISqlBuilder SetPager( int page, int pageSize = 20 ) {
            return SetPager( new Pager( page, pageSize ) );
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pager">分页对象</param>
        public ISqlBuilder SetPager( IPager pager ) {
            Pager = pager;
            if ( !pager.Order.IsEmpty() )
                Order = pager.Order;
            return this;
        }

        #endregion

        #region 连接

        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlBuilder And( ISqlCondition condition ) {
            return And( condition.GetCondition() );
        }

        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlBuilder And( string condition ) {
            if ( condition.IsEmpty() )
                return this;
            if ( _condition == null ) {
                _condition = SqlCondition.Create( condition );
                return this;
            }
            _condition = new AndCondition( _condition.GetCondition(), condition );
            return this;
        }

        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlBuilder Or( ISqlCondition condition ) {
            return Or( condition.GetCondition() );
        }

        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlBuilder Or( string condition ) {
            if ( condition.IsEmpty() )
                return this;
            if ( _condition == null ) {
                _condition = SqlCondition.Create( condition );
                return this;
            }
            _condition = new OrCondition( _condition.GetCondition(), condition );
            return this;
        }

        #endregion

        #region 过滤

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public ISqlBuilder Filter( string name, object value, Operator @operator = Operator.Equal ) {
            if ( !IsValid( name, value ) )
                return this;
            And( SqlCondition.Create( name, GetParamPrefix(), @operator ) );
            AddParam( name, value, @operator );
            return this;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        private bool IsValid( string name, object value ) {
            if ( name.IsEmpty() )
                return false;
            if ( value == null || value.ToString().IsEmpty() )
                return false;
            return true;
        }

        /// <summary>
        /// 获取参数前缀
        /// </summary>
        protected abstract string GetParamPrefix();

        /// <summary>
        /// 添加参数
        /// </summary>
        private void AddParam( string name, object value, Operator @operator ) {
            switch ( @operator ) {
                case Operator.Contains:
                    value = string.Format( "%{0}%", value );
                    break;
                case Operator.Starts:
                    value = string.Format( "{0}%", value );
                    break;
                case Operator.Ends:
                    value = string.Format( "%{0}", value );
                    break;
            }
            _params.Add( SqlCondition.GetParamName( name ), value );
        }

        /// <summary>
        /// 以相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Equal( string name, object value ) {
            Filter( name, value );
            return this;
        }

        /// <summary>
        /// 以不相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder NotEqual( string name, object value ) {
            Filter( name, value, Operator.NotEqual );
            return this;
        }

        /// <summary>
        /// 以大于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Greater( string name, object value ) {
            Filter( name, value, Operator.Greater );
            return this;
        }

        /// <summary>
        /// 以小于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Less( string name, object value ) {
            Filter( name, value, Operator.Less );
            return this;
        }

        /// <summary>
        /// 以大于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder GreaterEqual( string name, object value ) {
            Filter( name, value, Operator.GreaterEqual );
            return this;
        }

        /// <summary>
        /// 以小于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder LessEqual( string name, object value ) {
            Filter( name, value, Operator.LessEqual );
            return this;
        }

        /// <summary>
        /// 以头尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Contains( string name, object value ) {
            Filter( name, value, Operator.Contains );
            return this;
        }

        /// <summary>
        /// 以头匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Starts( string name, object value ) {
            Filter( name, value, Operator.Starts );
            return this;
        }

        /// <summary>
        /// 以尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlBuilder Ends( string name, object value ) {
            Filter( name, value, Operator.Ends );
            return this;
        }

        /// <summary>
        /// In过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="values">值</param>
        public ISqlBuilder In( string name, IEnumerable<object> values ) {
            if ( values == null )
                return this;
            var list = values.ToList().FindAll( t => !t.ToStr().IsEmpty() );
            if ( list.Count == 0 )
                return this;
            var result = new StringBuilder();
            result.AppendFormat( "{0} In (", name );
            for ( int i = 0; i < list.Count; i++ )
                AddIn( result, name, list[i], i );
            And( result.ToString().TrimEnd( ',' ) + ")" );
            return this;
        }

        /// <summary>
        /// 添加In过滤条件
        /// </summary>
        private void AddIn( StringBuilder result, string name, object value, int index ) {
            if ( !IsValid( name, value ) )
                return;
            var paramName = SqlCondition.GetParamName( string.Format( "{0}{1}", name, index ) );
            result.AppendFormat( "{0}{1},", GetParamPrefix(), paramName );
            _params.Add( paramName, value );
        }

        #endregion

        #region 辅助

        /// <summary>
        /// 获取Select子句
        /// </summary>
        protected string GetSelect() {
            return string.Format( "Select {0} ", GetColumns() );
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        protected string GetColumns() {
            if ( Columns.IsEmpty() )
                return "*";
            return Columns;
        }

        /// <summary>
        /// 获取From子句
        /// </summary>
        protected string GetFrom() {
            return string.Format( "From {0} ", Table );
        }

        /// <summary>
        /// 获取Where子句
        /// </summary>
        protected string GetWhere() {
            if ( _condition == null )
                return string.Empty;
            return string.Format( "Where {0} ", GetPredicate() );
        }

        /// <summary>
        /// 获取分组子句
        /// </summary>
        protected string GetGroupBy() {
            if ( Group.IsEmpty() )
                return string.Empty;
            var result = new StringBuilder();
            result.AppendFormat( "Group By {0}", Group );
            if ( !Having.IsEmpty() )
                result.AppendFormat( " Having {0}", Having );
            return result.ToString();
        }

        /// <summary>
        /// 获取排序子句
        /// </summary>
        protected string GetOrderBy() {
            if ( Order.IsEmpty() )
                return string.Empty;
            return string.Format( "Order By {0} ", Order );
        }

        #endregion

        #region 结果

        /// <summary>
        /// 获取谓词
        /// </summary>
        public string GetPredicate() {
            if ( _condition == null )
                return string.Empty;
            return _condition.GetCondition();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public IDictionary<string, object> GetParams() {
            return _params;
        }

        /// <summary>
        /// 生成Sql
        /// </summary>
        public string GetSql() {
            ValidateSql();
            var result = new StringBuilder();
            return GetSql( result );
        }

        /// <summary>
        /// 验证Sql有效性
        /// </summary>
        private void ValidateSql() {
            if ( Table.IsEmpty() )
                throw new InvalidOperationException( SqlResource.TableIsEmpty );
            if ( Pager != null && Order.IsEmpty() )
                throw new InvalidOperationException( SqlResource.PagerOrderIsEmpty );
        }

        /// <summary>
        /// 获取Sql
        /// </summary>
        protected abstract string GetSql( StringBuilder result );

        /// <summary>
        /// 生成获取总行数的Sql
        /// </summary>
        public string GetCountSql() {
            return string.Format( "Select Count(*) {0}{1}", GetFrom(), GetWhere() );
        }

        #endregion
    }
}
