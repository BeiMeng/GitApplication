using System;
using System.Collections.Generic;
using System.Linq;
using Util.Datas;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.ApplicationServices {
    /// <summary>
    /// 树型实体批操作服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    /// <typeparam name="TParentId">父编号类型</typeparam>
    public abstract class TreeBatchService<TEntity, TDto, TQuery, TKey, TParentId> : BatchService<TEntity, TDto, TQuery, TKey>, ITreeBatchService<TDto, TQuery>
        where TEntity : class, IAggregateRoot<TEntity, TKey>, ITreeEntity<TEntity, TKey, TParentId>, new()
        where TDto : IDto, new()
        where TQuery : IPager, ITreeEntityQuery {

        #region 构造方法

        /// <summary>
        /// 初始化批操作服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected TreeBatchService( IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository )
            : base( unitOfWork, repository ) {
            _pathChangeList = new List<TEntity>();
            _updatedPathIds = new List<TKey>();
        }

        #endregion

        #region 字段

        /// <summary>
        /// 需要更新路径的实体列表
        /// </summary>
        private readonly List<TEntity> _pathChangeList;

        /// <summary>
        /// 已更新路径Id列表
        /// </summary>
        private readonly List<TKey> _updatedPathIds;

        /// <summary>
        /// 更新前路径
        /// </summary>
        private string _pathBefore;

        #endregion

        #region GetParentIdsFromPath(从路径中获取所有上级节点编号)

        /// <summary>
        /// 从路径中获取所有上级节点编号
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        public List<string> GetParentIdsFromPath( TDto dto ) {
            return ToEntity( dto ).GetParentIdsFromPath().Select( t => t.ToString() ).ToList();
        }

        #endregion

        #region GetAllChilds(获取全部下级实体)

        /// <summary>
        /// 获取全部下级实体
        /// </summary>
        /// <param name="parent">父实体</param>
        protected virtual List<TEntity> GetAllChilds( TEntity parent ) {
            var list = Repository.Find().Where( t => t.Path.StartsWith( parent.Path ) ).ToList();
            return list.Where( t => !t.Id.Equals( parent.Id ) ).ToList();
        }

        #endregion

        #region GetChilds(获取直接下级)

        /// <summary>
        /// 获取直接下级
        /// </summary>
        /// <param name="parent">父实体</param>
        protected virtual List<TEntity> GetChilds( TEntity parent ) {
            throw new NotImplementedException();
        }

        #endregion

        #region GetRoots(获取根节点)

        /// <summary>
        /// 获取根节点
        /// </summary>
        protected virtual List<TEntity> GetRoots() {
            return Repository.Find().Where( t => t.Level == 1 ).ToList();
        }

        #endregion

        #region Create(创建实体)

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="parentId">父Id</param>
        public virtual TDto Create( string parentId ) {
            var entity = new TEntity { ParentId = Conv.To<TParentId>( parentId ) };
            entity.SortId = GetSortId( entity.ParentId );
            var dto = ToDto( entity );
            dto.Id = Guid.NewGuid().ToString();
            return dto;
        }

        /// <summary>
        /// 获取排序号
        /// </summary>
        /// <param name="parentId">父Id</param>
        protected virtual int GetSortId( TParentId parentId ) {
            return 1;
        }

        #endregion

        #region SaveBefore(保存前操作)

        /// <summary>
        /// 保存前操作
        /// </summary>
        protected override void SaveBefore( List<TDto> addList, List<TDto> updateList, List<TDto> deleteList ) {
            base.SaveBefore( addList, updateList, deleteList );
            var parentChangeList = GetParentChanges();
            GetPathChangeList( parentChangeList );
            InitPath();
        }

        /// <summary>
        /// 获取父节点被修改的集合
        /// </summary>
        private List<TEntity> GetParentChanges() {
            var result = new List<TEntity>();
            _updateList.ForEach( t => {
                var entity = Repository.Find( t.Id );
                if ( entity == null )
                    return;
                if ( t.ParentId.Equals( entity.ParentId ) )
                    return;
                result.Add( t );
            } );
            FilterByPath( result );
            return result;
        }

        /// <summary>
        /// 根据路径过滤，仅保留最顶级节点
        /// </summary>
        protected void FilterByPath( List<TEntity> entities ) {
            entities.Select( t => t.Path )
                .ToList()
                .ForEach( path => entities.RemoveAll( t => t.Path.StartsWith( path ) && t.Path != path ) );
        }

        /// <summary>
        /// 初始化需要更新路径的实体列表
        /// </summary>
        private void GetPathChangeList( List<TEntity> parentChangeList ) {
            AddPathChangeList( parentChangeList );
            AddPathChangeList( _addList );
            foreach ( var parent in parentChangeList ) {
                AddPathChangeList( _updateList.Where( t => t.Path.StartsWith( parent.Path ) ).ToList() );
                AddPathChangeList( GetAllChilds( parent ) );
            }
        }

        /// <summary>
        /// 添加需要更新路径的实体列表
        /// </summary>
        private void AddPathChangeList( List<TEntity> list ) {
            list.ForEach( entity => {
                if ( _pathChangeList.Any( t => t.Id.Equals( entity.Id ) ) )
                    return;
                _pathChangeList.Add( entity );
            } );
        }

        /// <summary>
        /// 初始化路径
        /// </summary>
        private void InitPath() {
            _pathChangeList.ForEach( InitPath );
        }

        /// <summary>
        /// 初始化路径
        /// </summary>
        private void InitPath( TEntity entity ) {
            if ( entity == null )
                return;
            if ( _updatedPathIds.Contains( entity.Id ) )
                return;
            UpdateParentPath( entity );
            InitEntityPath( entity );
        }

        //更新父节点路径
        private void UpdateParentPath( TEntity entity ) {
            if ( Equals( entity.ParentId, null ) )
                return;
            var parent = _pathChangeList.Find( t => t.Id.Equals( entity.ParentId ) );
            InitPath( parent );
        }

        /// <summary>
        /// 初始化实体路径
        /// </summary>
        private void InitEntityPath( TEntity entity ) {
            entity.InitPath( GetParent( entity ) );
            _updatedPathIds.Add( entity.Id );
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        private TEntity GetParent( TEntity entity ) {
            var result = _pathChangeList.Find( t => t.Id.Equals( entity.ParentId ) );
            return result ?? Repository.Find( entity.ParentId );
        }

        #endregion

        #region DeleteEntities(删除实体集合)

        /// <summary>
        /// 删除实体集合
        /// </summary>
        protected override void DeleteEntities( TEntity entity ) {
            base.DeleteEntities( entity );
            var childs = GetAllChilds( entity );
            if ( childs == null || childs.Count == 0 )
                return;
            childs.ForEach( DeleteEntity );
        }

        #endregion

        #region GetResult(获取结果)

        /// <summary>
        /// 获取结果
        /// </summary>
        protected override List<TDto> GetResult() {
            var ids = GetIds();
            var result = new List<TEntity>();
            ids.ForEach( id => result.Add( Repository.Find( id ) ) );
            return result.Select( ToDto ).ToList();
        }

        /// <summary>
        /// 获取标识集合
        /// </summary>
        private List<TKey> GetIds() {
            var ids = new List<TKey>();
            if ( _addList != null && _addList.Count > 0 )
                ids.AddRange( _addList.Select( t => Conv.To<TKey>( t.Id ) ) );
            if ( _updateList != null && _updateList.Count > 0 )
                ids.AddRange( _updateList.Select( t => Conv.To<TKey>( t.Id ) ) );
            return ids;
        }

        #endregion

        #region Enable(启用冻结)

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="ids">字典编号列表</param>
        public List<TDto> Enable( string ids ) {
            return Enable( ids, true );
        }

        /// <summary>
        /// 启用
        /// </summary>
        private List<TDto> Enable( string ids, bool isEnabled ) {
            ids.CheckNull( "ids" );
            var result = new List<TEntity>();
            var entities = Repository.Find( Conv.ToList<TKey>( ids ) );
            if ( entities == null )
                return result.Select( ToDto ).ToList();
            FilterByPath( entities );
            UnitOfWork.Start();
            entities.ForEach( entity => EnableNodeAndChilds( result, entity, isEnabled ) );
            UnitOfWork.Commit();
            WriteLog( result, isEnabled );
            return result.Select( ToDto ).ToList();
        }

        /// <summary>
        /// 启用节点及全部下级节点
        /// </summary>
        private void EnableNodeAndChilds( List<TEntity> result, TEntity entity, bool isEnabled ) {
            EnableNode( result, entity, isEnabled );
            var childs = GetAllChilds( entity );
            childs.ForEach( child => EnableNode( result, child, isEnabled ) );
        }

        /// <summary>
        /// 启用节点
        /// </summary>
        private void EnableNode( List<TEntity> result, TEntity entity, bool isEnabled ) {
            entity.Enabled = isEnabled;
            result.Add( entity );
        }

        /// <summary>
        /// 写日志
        /// </summary>
        private void WriteLog( List<TEntity> entities, bool isEnabled ) {
            AddLog( entities );
            WriteLog( string.Format( "{0}成功", isEnabled ? "启用" : "冻结" ) );
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="ids">编号列表</param>
        public List<TDto> Disable( string ids ) {
            return Enable( ids, false );
        }

        #endregion

        #region FixPath(修正路径)

        /// <summary>
        /// 修正路径
        /// </summary>
        public void FixPath() {
            UnitOfWork.Start();
            var roots = GetRoots();
            foreach ( var root in roots ) {
                FixPath( root );
                FixChildPath( root );
            }
            UnitOfWork.Commit();
        }

        /// <summary>
        /// 修正路径
        /// </summary>
        private void FixPath( TEntity entity, TEntity parent = null ) {
            _pathBefore = entity.Path;
            entity.InitPath( parent );
            if ( _pathBefore != entity.Path )
                WriteFixPathLog( entity );
        }

        /// <summary>
        /// 记录修正路径日志
        /// </summary>
        private void WriteFixPathLog( TEntity entity ) {
            Log.Caption.Add( "修正树型实体路径" );
            Log.BusinessId = entity.Id.ToString();
            Log.Content.AddLine( "修改前的错误路径:{0}", _pathBefore );
            Log.Content.AddLine( "修正后：" );
            Log.Content.Add( entity.ToString() );
            Log.Warn();
        }

        /// <summary>
        /// 修复下级子节点
        /// </summary>
        private void FixChildPath( TEntity parent ) {
            var childs = GetChilds( parent );
            if ( childs == null )
                return;
            foreach ( var child in childs ) {
                FixPath( child, parent );
                FixChildPath( child );
            }
        }

        #endregion
    }
}
