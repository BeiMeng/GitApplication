using System.Collections.Generic;
using Util.Datas.Sql.Queries.Builders.Conditions;
using Util.Domains.Repositories;

namespace Util.Datas.Sql.Queries.Builders {
    /// <summary>
    /// Sql生成器
    /// </summary>
    public interface ISqlBuilder {
        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名，可以多表连接，范例：a Join b On a.BId = b.Id</param>
        ISqlBuilder From( string table );
        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        ISqlBuilder Select( string columns );
        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">分组列名</param>
        /// <param name="having">分组条件</param>
        ISqlBuilder GroupBy( string group, string having = "" );
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="order">排序条件</param>
        ISqlBuilder OrderBy( string order );
        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数</param> 
        ISqlBuilder SetPager( int page, int pageSize = 20 );
        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pager">分页对象</param>
        ISqlBuilder SetPager( IPager pager );
        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlBuilder And( ISqlCondition condition );
        /// <summary>
        /// 使用And连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlBuilder And( string condition );
        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlBuilder Or( ISqlCondition condition );
        /// <summary>
        /// 使用Or连接条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        ISqlBuilder Or( string condition );
        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        ISqlBuilder Filter( string name, object value, Operator @operator = Operator.Equal );
        /// <summary>
        /// 以相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Equal( string name, object value );
        /// <summary>
        /// 以不相等条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder NotEqual( string name, object value );
        /// <summary>
        /// 以大于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Greater( string name, object value );
        /// <summary>
        /// 以小于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Less( string name, object value );
        /// <summary>
        /// 以大于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder GreaterEqual( string name, object value );
        /// <summary>
        /// 以小于等于条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder LessEqual( string name, object value );
        /// <summary>
        /// 以头尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Contains( string name, object value );
        /// <summary>
        /// 以头匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Starts( string name, object value );
        /// <summary>
        /// 以尾匹配条件过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="value">值</param>
        ISqlBuilder Ends( string name, object value );
        /// <summary>
        /// In过滤
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="values">值</param>
        ISqlBuilder In( string name, IEnumerable<object> values );
        /// <summary>
        /// 获取谓词
        /// </summary>
        string GetPredicate();
        /// <summary>
        /// 获取参数
        /// </summary>
        IDictionary<string, object> GetParams();
        /// <summary>
        /// 生成Sql
        /// </summary>
        string GetSql();
        /// <summary>
        /// 生成获取总行数的Sql
        /// </summary>
        string GetCountSql();
    }
}
