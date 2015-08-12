using System.Collections.Generic;
using System.Data;
using Util.Datas.Sql.Queries.Builders;
using Util.Domains.Repositories;

namespace Util.Datas.Sql.Queries {
    /// <summary>
    /// 基于Sql语句的查询对象
    /// </summary>
    public interface ISqlQuery {
        /// <summary>
        /// 创建一个新的Sql生成器
        /// </summary>
        ISqlBuilder NewSqlBuilder();
        /// <summary>
        /// 获取Sql生成器
        /// </summary>
        ISqlBuilder GetSqlBuilder();
        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        ISqlQuery Select( string columns );
        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">分组列名</param>
        /// <param name="having">分组条件</param>
        ISqlQuery GroupBy( string group, string having = "" );
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="order">排序条件</param>
        ISqlQuery OrderBy( string order );
        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlQuery And( string condition );
        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlQuery Or( string condition );
        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        ISqlQuery Filter( string name, object value, Operator @operator = Operator.Equal );
        /// <summary>
        /// 以相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Equal( string name, object value );
        /// <summary>
        /// 以不相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery NotEqual( string name, object value );
        /// <summary>
        /// 以大于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Greater( string name, object value );
        /// <summary>
        /// 以小于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Less( string name, object value );
        /// <summary>
        /// 以大于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery GreaterEqual( string name, object value );
        /// <summary>
        /// 以小于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery LessEqual( string name, object value );
        /// <summary>
        /// 以头尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Contains( string name, object value );
        /// <summary>
        /// 以头匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Starts( string name, object value );
        /// <summary>
        /// 以尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlQuery Ends( string name, object value );
        /// <summary>
        /// In过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="values">值</param>
        ISqlQuery In( string name, IEnumerable<object> values );
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        List<T> GetList<T>( string table,IDbConnection connection );
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数</param> 
        PagerList<T> GetPageList<T>( string table, IDbConnection connection, int page, int pageSize = 20 );
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="pager">分页对象</param>
        PagerList<T> GetPageList<T>( string table,IDbConnection connection,IPager pager );
    }
}
