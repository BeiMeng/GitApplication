using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util.Datas.Queries.Criterias;
using Util.Datas.Queries.OrderBys;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.Queries {
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class Query<TEntity, TKey> : Pager, IQuery<TEntity, TKey> where TEntity : class ,IAggregateRoot {

        #region 构造方法

        /// <summary>
        /// 初始化查询对象
        /// </summary>
        public Query() {
            OrderBuilder = new OrderByBuilder();
        }

        /// <summary>
        /// 初始化查询对象
        /// </summary>
        /// <param name="pager">分页对象</param>
        public Query( IPager pager ) : this() {
            Page = pager.Page;
            PageSize = pager.PageSize;
            TotalCount = pager.TotalCount;
            OrderBy( pager.Order );
        }

        #endregion

        #region 属性

        /// <summary>
        /// 查询条件
        /// </summary>
        private ICriteria<TEntity> Criteria { get; set; }

        /// <summary>
        /// 排序生成器
        /// </summary>
        private OrderByBuilder OrderBuilder { get; set; }

        #endregion

        #region GetPredicate(获取谓词)

        /// <summary>
        /// 获取谓词
        /// </summary>
        public Expression<Func<TEntity, bool>> GetPredicate() {
            if ( Criteria == null )
                return null;
            return Criteria.GetPredicate();
        }

        #endregion

        #region GetOrderBy(获取排序)

        /// <summary>
        /// 获取排序
        /// </summary>
        public string GetOrderBy() {
            Order = OrderBuilder.Generate();
            if ( string.IsNullOrWhiteSpace( Order ) )
                Order = "Id desc";
            return Order;
        }

        #endregion

        #region 过滤条件

        /// <summary>
        /// 添加谓词,仅能添加一个条件,如果参数值为空，则忽略该条件
        /// </summary>
        /// <param name="predicate">谓词</param>
        /// <param name="isOr">是否使用Or连接</param>
        public IQuery<TEntity, TKey> Filter( Expression<Func<TEntity, bool>> predicate,bool isOr = false ) {
            predicate = QueryHelper.ValidatePredicate( predicate );
            if ( predicate == null )
                return this;
            if ( isOr )
                Or( predicate );
            else 
                And( predicate );
            return this;
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public IQuery<TEntity, TKey> Filter( string propertyName, object value, Operator @operator = Operator.Equal ) {
            return Filter( Lambda.ParsePredicate<TEntity>( propertyName, value, @operator ) );
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="criteria">查询条件</param>
        public IQuery<TEntity, TKey> Filter( ICriteria<TEntity> criteria ) {
            And( criteria.GetPredicate() );
            return this;
        }

        /// <summary>
        /// 过滤int数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public IQuery<TEntity, TKey> FilterInt<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, int? min, int? max ) {
            return Filter( new IntSegmentCriteria<TEntity, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤double数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public IQuery<TEntity, TKey> FilterDouble<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, double? min, double? max ) {
            return Filter( new DoubleSegmentCriteria<TEntity, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤日期段，不包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public IQuery<TEntity, TKey> FilterDate<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max ) {
            return Filter( new DateSegmentCriteria<TEntity, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤日期时间段，包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public IQuery<TEntity, TKey> FilterDateTime<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max ) {
            return Filter( new DateTimeSegmentCriteria<TEntity, TProperty>( propertyExpression, min, max ) );
        }

        /// <summary>
        /// 过滤decimal数值段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public IQuery<TEntity, TKey> FilterDecimal<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression, decimal? min, decimal? max ) {
            return Filter( new DecimalSegmentCriteria<TEntity, TProperty>( propertyExpression, min, max ) );
        }

        #endregion

        #region 连接

        /// <summary>
        /// 与连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="query">查询对象</param>
        public IQuery<TEntity, TKey> And( IQuery<TEntity, TKey> query ) {
            return And( query.GetPredicate() );
        }

        /// <summary>
        /// 与连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="predicate">谓词</param>
        public IQuery<TEntity, TKey> And( Expression<Func<TEntity, bool>> predicate ) {
            if ( Criteria == null ) {
                Criteria = new Criteria<TEntity>( predicate );
                return this;
            }
            Criteria = new AndCriteria<TEntity>( Criteria.GetPredicate(), predicate );
            return this;
        }

        /// <summary>
        /// 或连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="query">查询对象</param>
        public IQuery<TEntity, TKey> Or( IQuery<TEntity, TKey> query ) {
            return Or( query.GetPredicate() );
        }

        /// <summary>
        /// 或连接,将传入的查询条件合并到当前对象
        /// </summary>
        /// <param name="predicate">谓词</param>
        public IQuery<TEntity, TKey> Or( Expression<Func<TEntity, bool>> predicate ) {
            if ( Criteria == null ) {
                Criteria = new Criteria<TEntity>( predicate );
                return this;
            }
            Criteria = new OrCriteria<TEntity>( Criteria.GetPredicate(), predicate );
            return this;
        }

        #endregion

        #region OrderBy(排序)

        /// <summary>
        /// 添加排序，支持多次调用OrderBy创建多级排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">属性表达式</param>
        /// <param name="desc">是否降序</param>
        public IQuery<TEntity, TKey> OrderBy<TProperty>( Expression<Func<TEntity, TProperty>> expression, bool desc = false ) {
            return OrderBy( Lambda.GetName( expression ), desc );
        }

        /// <summary>
        /// 添加排序，支持多次调用OrderBy创建多级排序
        /// </summary>
        /// <param name="propertyName">排序属性</param>
        /// <param name="desc">是否降序</param>
        public IQuery<TEntity, TKey> OrderBy( string propertyName, bool desc = false ) {
            OrderBuilder.Add( propertyName, desc );
            GetOrderBy();
            return this;
        }

        #endregion

        #region Clear(清理)

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear() {
            Criteria = null;
            OrderBuilder = new OrderByBuilder();
        }

        #endregion

        #region GetList(获取列表)

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryable">数据源</param>
        public List<TEntity> GetList( IQueryable<TEntity> queryable ) {
            return Execute( queryable ).OrderBy( Order ).ToList();
        }

        /// <summary>
        /// 执行过滤和分页
        /// </summary>
        private IQueryable<TEntity> Execute( IQueryable<TEntity> queryable ) {
            queryable.CheckNull( "queryable" );
            queryable = FilterBy( queryable );
            GetOrderBy();
            return queryable;
        }

        /// <summary>
        /// 过滤
        /// </summary>
        private IQueryable<TEntity> FilterBy( IQueryable<TEntity> queryable ) {
            if ( Criteria == null )
                return queryable;
            return queryable.Where( Criteria.GetPredicate() );
        }

        #endregion

        #region GetPagerList(获取分页列表)

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryable">数据源</param>
        public PagerList<TEntity> GetPagerList( IQueryable<TEntity> queryable ) {
            return Execute( queryable ).PagerResult( this );
        }

        #endregion
    }
}
