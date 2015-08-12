using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util.Datas;

namespace Util.Domains.Repositories {
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IRepository<TEntity, in TKey> : IDependency where TEntity : class, IAggregateRoot {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Add( TEntity entity );
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entities">实体</param>
        void Add( IEnumerable<TEntity> entities );
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update( TEntity entity );
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="newEntity">新实体</param>
        /// <param name="oldEntity">数据库中旧的实体</param>
        void Update( TEntity newEntity,TEntity oldEntity );
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">实体标识</param>
        void Remove( TKey id );
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Remove( TEntity entity );
        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="ids">实体编号集合</param>
        void Remove( IEnumerable<TKey> ids );
        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Remove( IEnumerable<TEntity> entities );
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="predicate">条件</param>
        void Remove( Expression<Func<TEntity, bool>> predicate );
        /// <summary>
        /// 查找实体集合
        /// </summary>
        List<TEntity> FindAll();
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate">条件</param>
        TEntity Single( Expression<Func<TEntity, bool>> predicate );
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        IQueryable<TEntity> FindAsNoTracking();
        /// <summary>
        /// 查找实体集合
        /// </summary>
        IQueryable<TEntity> Find();
        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="criteria">条件</param>
        IQueryable<TEntity> Find( ICriteria criteria );
        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="criteria">条件</param>
        IQueryable<TEntity> Find( ICriteria<TEntity> criteria );
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id">实体标识</param>
        TEntity Find( params object[] id );
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="ids">实体标识列表</param>
        List<TEntity> Find( IEnumerable<TKey> ids );
        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="predicate">条件</param>
        IQueryable<TEntity> Find( Expression<Func<TEntity, bool>> predicate );
        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="predicate">条件</param>
        List<TEntity> FindList( Expression<Func<TEntity, bool>> predicate );
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        bool Exists( Expression<Func<TEntity, bool>> predicate );
        /// <summary>
        /// 索引器查找，获取指定标识的实体
        /// </summary>
        /// <param name="id">实体标识</param>
        TEntity this[TKey id] { get; }
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        /// <param name="query">查询对象</param>
        IQueryable<TEntity> QueryAsNoTracking( IQueryBase<TEntity> query );
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        IQueryable<TEntity> Query( IQueryBase<TEntity> query );
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询对象</param>
        PagerList<TEntity> PagerQuery( IQueryBase<TEntity> query );
        /// <summary>
        /// 保存
        /// </summary>
        void Save();
        /// <summary>
        /// 获取实体个数
        /// </summary>
        /// <param name="predicate">条件</param>
        int Count( Expression<Func<TEntity, bool>> predicate = null );
        /// <summary>
        /// 清空实体
        /// </summary>
        void Clear();
        /// <summary>
        /// 清空缓存
        /// </summary>
        void ClearCache();
        /// <summary>
        /// 获取工作单元
        /// </summary>
        IUnitOfWork GetUnitOfWork();
    }
}
