using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util.Datas.Queries;
using Util.Datas.Queries.Criterias;
using Util.Domains;
using Util.Domains.Repositories;
using Util.Lambdas.Dynamics;

namespace Util.Datas {
    /// <summary>
    /// 查询扩展
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="predicate">谓词</param>
        public static IQueryable<T> Filter<T>( this IQueryable<T> source, Expression<Func<T, bool>> predicate ) {
            predicate = QueryHelper.ValidatePredicate( predicate );
            if ( predicate == null )
                return source;
            return source.Where( predicate );
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="filter">查询过滤器</param>
        public static IQueryable<T> Filter<T>( this IQueryable<T> source, IQueryableFilter<T> filter ) {
            if ( filter == null )
                return source;
            return filter.Filter( source );
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="criteria">查询条件</param>
        public static IQueryable<T> Filter<T>( this IQueryable<T> source, ICriteria<T> criteria ) where T : class,IAggregateRoot {
            if ( criteria == null )
                return source;
            var predicate = criteria.GetPredicate();
            if ( predicate == null )
                return source;
            return source.Where( predicate );
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="criteria">查询条件</param>
        public static IQueryable<T> Filter<T>( this IQueryable<T> source, ICriteria criteria ) where T : class,IAggregateRoot {
            if ( criteria == null )
                return source;
            var predicate = criteria.GetPredicate();
            if ( predicate.IsEmpty() )
                return source;
            return source.Where( predicate,criteria.GetValues() );
        }

        /// <summary>
        /// 过滤整数段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static IQueryable<T> FilterInt<T, TProperty>( this IQueryable<T> source,
            Expression<Func<T, TProperty>> propertyExpression, int? min, int? max ) where T : class,IAggregateRoot {
            return source.Filter( new IntSegmentCriteria<T, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤double数值段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static IQueryable<T> FilterDouble<T, TProperty>( this IQueryable<T> source,
            Expression<Func<T, TProperty>> propertyExpression, double? min, double? max ) where T : class,IAggregateRoot {
            return source.Filter( new DoubleSegmentCriteria<T, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤decimal数值段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static IQueryable<T> FilterDecimal<T, TProperty>( this IQueryable<T> source,
            Expression<Func<T, TProperty>> propertyExpression, decimal? min, decimal? max ) where T : class,IAggregateRoot {
            return source.Filter( new DecimalSegmentCriteria<T, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤日期段，不包含时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static IQueryable<T> FilterDate<T, TProperty>( this IQueryable<T> source,
            Expression<Func<T, TProperty>> propertyExpression, DateTime? min, DateTime? max ) where T : class,IAggregateRoot {
            return source.Filter( new DateSegmentCriteria<T, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤日期时间段，包含时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static IQueryable<T> FilterDateTime<T, TProperty>( this IQueryable<T> source,
            Expression<Func<T, TProperty>> propertyExpression, DateTime? min, DateTime? max ) where T : class,IAggregateRoot {
            return source.Filter( new DateTimeSegmentCriteria<T, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="propertyName">排序属性名，多个属性用逗号分隔，降序用desc字符串，范例：Name,Age desc</param>
        public static IQueryable<T> OrderBy<T>( this IQueryable<T> source, string propertyName ) {
            return source.OrderByDynamic( propertyName );
        }

        /// <summary>
        /// 创建分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="page">页索引，表示第几页，从1开始</param>
        /// <param name="pageSize">每页显示行数，默认20</param>
        public static PagerList<T> PagerResult<T>( this IQueryable<T> source, int page,int pageSize = 20 ) {
            return PagerResult( source, new Pager( page, pageSize ) );
        }

        /// <summary>
        /// 创建分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pager">分页对象</param>
        public static PagerList<T> PagerResult<T>( this IQueryable<T> source, IPager pager ) {
            return OrderBy( source, pager ).Pager( pager ).ToPageList( pager );
        }

        /// <summary>
        /// 排序
        /// </summary>
        private static IQueryable<T> OrderBy<T>( IQueryable<T> source, IPager pager ) {
            if ( pager.Order.IsEmpty() )
                return source;
            return source.OrderBy( pager.Order );
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pager">分页对象</param>
        public static IQueryable<T> Pager<T>( this IQueryable<T> source, IPager pager ) {
            if ( pager.TotalCount <= 0 )
                pager.TotalCount = source.Count();
            return source.Skip( pager.GetSkipCount() ).Take( pager.PageSize );
        }

        /// <summary>
        /// 转换为分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pager">分页对象</param>
        public static PagerList<T> ToPageList<T>( this IEnumerable<T> source, IPager pager ) {
            var result = new PagerList<T>( pager );
            result.AddRange( source.ToList() );
            return result;
        }
    }
}
