using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dappers;
using Util.Datas.Sql.Queries.Builders;
using Util.Domains.Repositories;

namespace Util.Datas.Sql.Queries {
    /// <summary>
    /// 基于Sql语句的查询对象
    /// </summary>
    public abstract class SqlQueryBase : ISqlQuery {
        /// <summary>
        /// Sql生成器
        /// </summary>
        private ISqlBuilder _builder;

        /// <summary>
        /// 创建一个新的Sql生成器
        /// </summary>
        public ISqlBuilder NewSqlBuilder() {
            return CreateSqlBuilder();
        }

        /// <summary>
        /// 创建Sql生成器
        /// </summary>
        protected abstract ISqlBuilder CreateSqlBuilder();

        /// <summary>
        /// 获取Sql生成器
        /// </summary>
        public ISqlBuilder GetSqlBuilder() {
            return _builder ?? ( _builder = CreateSqlBuilder() );
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        public ISqlQuery Select( string columns ) {
            GetSqlBuilder().Select( columns );
            return this;
        }

        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">分组列名</param>
        /// <param name="having">分组条件</param>
        public ISqlQuery GroupBy( string group, string having = "" ) {
            GetSqlBuilder().GroupBy( group, having );
            return this;
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="order">排序条件</param>
        public ISqlQuery OrderBy( string order ) {
            GetSqlBuilder().OrderBy( order );
            return this;
        }

        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlQuery And( string condition ) {
            GetSqlBuilder().And( condition );
            return this;
        }

        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        public ISqlQuery Or( string condition ) {
            GetSqlBuilder().Or( condition );
            return this;
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public ISqlQuery Filter( string name, object value, Operator @operator = Operator.Equal ) {
            GetSqlBuilder().Filter( name, value, @operator );
            return this;
        }

        /// <summary>
        /// 以相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Equal( string name, object value ) {
            GetSqlBuilder().Equal( name, value );
            return this;
        }

        /// <summary>
        /// 以不相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery NotEqual( string name, object value ) {
            GetSqlBuilder().NotEqual( name, value );
            return this;
        }

        /// <summary>
        /// 以大于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Greater( string name, object value ) {
            GetSqlBuilder().Greater( name, value );
            return this;
        }

        /// <summary>
        /// 以小于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Less( string name, object value ) {
            GetSqlBuilder().Less( name, value );
            return this;
        }

        /// <summary>
        /// 以大于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery GreaterEqual( string name, object value ) {
            GetSqlBuilder().GreaterEqual( name, value );
            return this;
        }

        /// <summary>
        /// 以小于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery LessEqual( string name, object value ) {
            GetSqlBuilder().LessEqual( name, value );
            return this;
        }

        /// <summary>
        /// 以头尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Contains( string name, object value ) {
            GetSqlBuilder().Contains( name, value );
            return this;
        }

        /// <summary>
        /// 以头匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Starts( string name, object value ) {
            GetSqlBuilder().Starts( name, value );
            return this;
        }

        /// <summary>
        /// 以尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        public ISqlQuery Ends( string name, object value ) {
            GetSqlBuilder().Ends( name, value );
            return this;
        }

        /// <summary>
        /// In过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="values">值</param>
        public ISqlQuery In( string name, IEnumerable<object> values ) {
            GetSqlBuilder().In( name, values );
            return this;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        public List<T> GetList<T>( string table, IDbConnection connection ) {
            var builder = GetSqlBuilder().From( table );
            return connection.Query<T>( builder.GetSql(), builder.GetParams() ).ToList();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数</param> 
        public PagerList<T> GetPageList<T>( string table, IDbConnection connection, int page, int pageSize = 20 ) {
            return GetPageList<T>( table, connection, new Pager( page, pageSize ) );
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="pager">分页对象</param>
        public PagerList<T> GetPageList<T>( string table, IDbConnection connection, IPager pager ) {
            GetSqlBuilder().From( table ).SetPager( pager );
            Count( connection, pager );
            return PagerQuery<T>( connection, pager );
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        private void Count( IDbConnection connection, IPager pager ) {
            if ( pager.TotalCount > 0 )
                return;
            pager.TotalCount = connection.ExecuteScalar<int>( _builder.GetCountSql(), _builder.GetParams() );
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        private PagerList<T> PagerQuery<T>( IDbConnection connection, IPager pager ) {
            return connection.Query<T>( _builder.GetSql(), _builder.GetParams() ).ToPageList( pager );
        }
    }
}
