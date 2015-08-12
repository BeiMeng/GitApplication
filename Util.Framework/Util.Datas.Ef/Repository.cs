using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.Ef {
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<TEntity, TKey> {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        protected Repository( IUnitOfWork unitOfWork ) {
            UnitOfWork = (EfUnitOfWork)unitOfWork;
        }

        /// <summary>
        /// Ef工作单元
        /// </summary>
        protected EfUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        protected IDbConnection Connection {
            get { return UnitOfWork.Database.Connection; }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Add( TEntity entity ) {
            UnitOfWork.Set<TEntity>().Add( entity );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entities">实体</param>
        public void Add( IEnumerable<TEntity> entities ) {
            if ( entities == null )
                return;
            UnitOfWork.Set<TEntity>().AddRange( entities );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Update( TEntity entity ) {
            UnitOfWork.Entry( entity ).State = EntityState.Modified;
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="newEntity">新实体</param>
        /// <param name="oldEntity">数据库中旧的实体</param>
        public void Update( TEntity newEntity, TEntity oldEntity ) {
            UnitOfWork.Entry( oldEntity ).CurrentValues.SetValues( newEntity );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public void Remove( TKey id ) {
            var entity = Find( id );
            if ( entity == null )
                return;
            Remove( entity );
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Remove( TEntity entity ) {
            UnitOfWork.Set<TEntity>().Remove( entity );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="ids">实体编号集合</param>
        public void Remove( IEnumerable<TKey> ids ) {
            if ( ids == null )
                return;
            Remove( Find( ids.ToArray() ) );
        }

        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void Remove( IEnumerable<TEntity> entities ) {
            if ( entities == null )
                return;
            var list = entities.ToList();
            if ( !list.Any() )
                return;
            UnitOfWork.Set<TEntity>().RemoveRange( list );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="predicate">条件</param>
        public void Remove( Expression<Func<TEntity, bool>> predicate ) {
            var entities = UnitOfWork.Set<TEntity>().Where( predicate );
            Remove( entities );
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        public List<TEntity> FindAll() {
            return Find().ToList();
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate">条件</param>
        public TEntity Single( Expression<Func<TEntity, bool>> predicate ) {
            return Find().FirstOrDefault( predicate );
        }

        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        public IQueryable<TEntity> FindAsNoTracking() {
            return UnitOfWork.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        public IQueryable<TEntity> Find() {
            return UnitOfWork.Set<TEntity>();
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="criteria">条件</param>
        public IQueryable<TEntity> Find( ICriteria criteria ) {
            return Find().Filter( criteria );
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="criteria">条件</param>
        public IQueryable<TEntity> Find( ICriteria<TEntity> criteria ) {
            return Find().Filter( criteria );
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public TEntity Find( params object[] id ) {
            return UnitOfWork.Set<TEntity>().Find( id );
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="ids">实体标识列表</param>
        public List<TEntity> Find( IEnumerable<TKey> ids ) {
            if ( ids == null )
                return null;
            return Find().Where( t => ids.Contains( t.Id ) ).ToList();
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="predicate">条件</param>
        public IQueryable<TEntity> Find( Expression<Func<TEntity, bool>> predicate ) {
            return Find().Where( predicate );
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="predicate">条件</param>
        public List<TEntity> FindList( Expression<Func<TEntity, bool>> predicate ) {
            return Find( predicate ).ToList();
        }

        /// <summary>
        /// 索引器查找，获取指定标识的实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public TEntity this[TKey id] {
            get { return Find( id ); }
        }

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public bool Exists( Expression<Func<TEntity, bool>> predicate ) {
            return Find().Any( predicate );
        }

        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        /// <param name="query">查询对象</param>
        public IQueryable<TEntity> QueryAsNoTracking( IQueryBase<TEntity> query ) {
            return FilterBy( FindAsNoTracking(), query );
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        public IQueryable<TEntity> Query( IQueryBase<TEntity> query ) {
            return FilterBy( Find(), query );
        }

        /// <summary>
        /// 过滤
        /// </summary>
        protected IQueryable<TEntity> FilterBy( IQueryable<TEntity> queryable, IQueryBase<TEntity> query ) {
            var predicate = query.GetPredicate();
            if ( predicate == null )
                return queryable;
            return queryable.Where( predicate );
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询对象</param>
        public virtual PagerList<TEntity> PagerQuery( IQueryBase<TEntity> query ) {
            return Query( query ).PagerResult( query );
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save() {
            UnitOfWork.Commit();
        }

        /// <summary>
        /// 获取实体个数
        /// </summary>
        /// <param name="predicate">条件</param>
        public int Count( Expression<Func<TEntity, bool>> predicate = null ) {
            if ( predicate == null )
                return UnitOfWork.Set<TEntity>().Count();
            return UnitOfWork.Set<TEntity>().Count( predicate );
        }

        /// <summary>
        /// 清空实体
        /// </summary>
        public virtual void Clear() {
            foreach ( var entity in UnitOfWork.Set<TEntity>() )
                UnitOfWork.Set<TEntity>().Remove( entity );
            UnitOfWork.CommitByStart();
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache() {
            UnitOfWork.ChangeTracker.Entries().ToList().ForEach( entry => entry.State = EntityState.Detached );
        }

        /// <summary>
        /// 获取工作单元
        /// </summary>
        public IUnitOfWork GetUnitOfWork() {
            return UnitOfWork;
        }
    }
}
