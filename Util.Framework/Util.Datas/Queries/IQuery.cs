using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.Queries {
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IQuery<TEntity, TKey> : IQueryBase<TEntity> where TEntity : class,IAggregateRoot {
        /// <summary>
        /// 添加谓词,仅能添加一个条件,如果参数值为空，则忽略该条件
        /// </summary>
        /// <param name="predicate">谓词</param>
        /// <param name="isOr">是否使用Or连接</param>
        IQuery<TEntity, TKey> Filter( Expression<Func<TEntity, bool>> predicate, bool isOr = false );

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        IQuery<TEntity,TKey> Filter( string propertyName, object value, Operator @operator = Operator.Equal );

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="criteria">查询条件</param>
        IQuery<TEntity,TKey> Filter( ICriteria<TEntity> criteria );

        /// <summary>
        /// 过滤int数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity,TKey> FilterInt<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, int? min,int? max );

        /// <summary>
        /// 过滤double数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity,TKey> FilterDouble<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, double? min,
            double? max );

        /// <summary>
        /// 过滤日期段，不包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity,TKey> FilterDate<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min,
            DateTime? max );

        /// <summary>
        /// 过滤日期时间段，包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity,TKey> FilterDateTime<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression,
            DateTime? min, DateTime? max );

        /// <summary>
        /// 过滤decimal数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity,TKey> FilterDecimal<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, decimal? min,
            decimal? max );

        /// <summary>
        /// 与连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="query">查询对象</param>
        IQuery<TEntity,TKey> And( IQuery<TEntity,TKey> query );

        /// <summary>
        /// 与连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="predicate">谓词</param>
        IQuery<TEntity,TKey> And( Expression<Func<TEntity, bool>> predicate );

        /// <summary>
        /// 或连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="query">查询对象</param>
        IQuery<TEntity,TKey> Or( IQuery<TEntity,TKey> query );

        /// <summary>
        /// 或连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="predicate">谓词</param>
        IQuery<TEntity,TKey> Or( Expression<Func<TEntity, bool>> predicate );

        /// <summary>
        /// 添加排序，支持多次调用OrderBy创建多级排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">属性表达式</param>
        /// <param name="desc">是否降序</param>
        IQuery<TEntity,TKey> OrderBy<TProperty>( Expression<Func<TEntity, TProperty>> expression, bool desc = false );

        /// <summary>
        /// 添加排序，支持多次调用OrderBy创建多级排序
        /// </summary>
        /// <param name="propertyName">排序属性</param>
        /// <param name="desc">是否降序</param>
        IQuery<TEntity,TKey> OrderBy( string propertyName, bool desc = false );

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryable">数据源</param>
        List<TEntity> GetList( IQueryable<TEntity> queryable );

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryable">数据源</param>
        PagerList<TEntity> GetPagerList( IQueryable<TEntity> queryable );
    }
}
