using System.Collections.Generic;
using System.Linq;
using Util.Datas;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.ApplicationServices {
    /// <summary>
    /// 批操作服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class BatchService<TEntity, TDto, TQuery, TKey> : ServiceBase<TEntity, TDto, TQuery, TKey>, IBatchService<TDto, TQuery>
        where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
        where TDto : IDto, new()
        where TQuery : IPager {
        /// <summary>
        /// 初始化批操作服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected BatchService( IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository )
            : base( unitOfWork, repository ) {
            _addList = new List<TEntity>();
            _updateList = new List<TEntity>();
            _deleteList = new List<TEntity>();
        }

        /// <summary>
        /// 新增列表
        /// </summary>
        protected List<TEntity> _addList;
        /// <summary>
        /// 修改列表
        /// </summary>
        protected List<TEntity> _updateList;
        /// <summary>
        /// 删除列表
        /// </summary>
        protected List<TEntity> _deleteList;

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="addList">新增列表</param>
        /// <param name="updateList">修改列表</param>
        /// <param name="deleteList">删除列表</param>
        public List<TDto> Save( List<TDto> addList, List<TDto> updateList, List<TDto> deleteList ) {
            UnitOfWork.Start();
            SaveBefore( addList, updateList, deleteList );
            AddList();
            UpdateList();
            DeleteList();
            UnitOfWork.Commit();
            SaveAfter();
            return GetResult();
        }

        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="addList">新增列表</param>
        /// <param name="updateList">修改列表</param>
        /// <param name="deleteList">删除列表</param>
        protected virtual void SaveBefore( List<TDto> addList, List<TDto> updateList, List<TDto> deleteList ) {
            FilterList( addList, deleteList );
            FilterList( updateList, deleteList );
            _addList = addList.Select( ToEntity ).Distinct().ToList();
            _updateList = updateList.Select( ToEntity ).Distinct().ToList();
            _deleteList = deleteList.Select( ToEntity ).Distinct().ToList();
        }

        /// <summary>
        /// 过滤无效数据
        /// </summary>
        private void FilterList( List<TDto> list, IEnumerable<TDto> deleteList ) {
            list.Select( t => t.Id ).ToList().ForEach( id => {
                if ( deleteList.Any( d => d.Id == id ) )
                    list.Remove( list.Find( t => t.Id == id ) );
            } );
        }

        /// <summary>
        /// 添加列表
        /// </summary>
        private void AddList() {
            if ( _addList == null || _addList.Count == 0 )
                return;
            LogContent.AddLine( "添加实体：" );
            _addList.ForEach( Add );
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        private void UpdateList() {
            if ( _updateList == null || _updateList.Count == 0 )
                return;
            LogContent.AddLine( "修改实体：" );
            _updateList.ForEach( Update );
        }

        /// <summary>
        /// 删除列表
        /// </summary>
        private void DeleteList() {
            if ( _deleteList == null || _deleteList.Count == 0 )
                return;
            LogContent.AddLine( "删除实体：" );
            _deleteList.ForEach( DeleteEntity );
        }

        /// <summary>
        /// 删除实体集合
        /// </summary>
        protected virtual void DeleteEntities( TEntity entity ) {
            DeleteEntity( entity );
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        protected void DeleteEntity( TEntity entity ) {
            Repository.Remove( entity.Id );
            AddLog( entity );
        }

        /// <summary>
        /// 保存后操作
        /// </summary>
        protected virtual void SaveAfter() {
            WriteLog( string.Format( "保存{0}成功", DisplayName ) );
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        protected virtual List<TDto> GetResult() {
            return new List<TDto>();
        }
    }
}
