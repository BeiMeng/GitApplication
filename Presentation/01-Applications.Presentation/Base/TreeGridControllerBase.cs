using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Util;
using Util.ApplicationServices;
using Util.Domains;
using Util.Domains.Repositories;
using Util.Webs.Controls;
using Util.Webs.EasyUi.Trees;
using Util.Webs.Mvc;

namespace Presentation.Base {
    /// <summary>
    /// 树型表格控制器
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    public abstract class TreeGridControllerBase<TDto, TQuery> : GridControllerBase<TDto, TQuery>
        where TQuery : IPager, ITreeEntityQuery, new()
        where TDto : class ,IDto, ITreeNode, new() {

        #region 构造方法

        /// <summary>
        /// 初始化树型表格控制器
        /// </summary>
        /// <param name="service">服务</param>
        protected TreeGridControllerBase( ITreeBatchService<TDto, TQuery> service )
            : base( service ) {
            Service = service;
        }

        #endregion

        #region Service(服务)

        /// <summary>
        /// 服务
        /// </summary>
        protected new ITreeBatchService<TDto, TQuery> Service { get; private set; }

        #endregion

        #region Ok(返回成功消息)

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        protected override ActionResult Ok( string message = "操作成功", IEnumerable<object> data = null ) {
            if ( LoadMode == LoadMode.Sync )
                return base.Ok( message, data );
            return base.Ok( message, GetNodes( data ) );
        }

        /// <summary>
        /// 获取树型节点
        /// </summary>
        private IEnumerable<object> GetNodes( IEnumerable<object> data ) {
            if ( data == null )
                return null;
            var list = data.ToList();
            foreach ( var item in list )
                UpdateStateToClosed( item );
            return list;
        }

        /// <summary>
        /// 修改节点状态
        /// </summary>
        private void UpdateStateToClosed( object item ) {
            var node = item as ITreeNode;
            if ( node == null )
                return;
            if ( LoadMode == LoadMode.RootAsync && node.Level > 1 )
                return;
            node.State = TreeNodeState.Closed.Description();
        }

        #endregion

        #region LoadMode(获取加载模式)

        /// <summary>
        /// 获取加载模式
        /// </summary>
        protected virtual LoadMode LoadMode {
            get { return LoadMode.Async; }
        }

        #endregion

        #region New(获取新实体)

        /// <summary>
        /// 获取新实体
        /// </summary>
        /// <param name="parentId">父Id</param>
        public override ActionResult New( string parentId ) {
            return ToJsonResult( Service.Create( parentId ) );
        }

        #endregion

        #region Query(查询)

        /// <summary>
        /// 查询
        /// </summary>
        public override ActionResult Query( TQuery query ) {
            if ( IsLoadChilds() )
                return GetChildsResult();
            if ( IsQuery( query ) )
                return GetQueryResult( query );
            return GetRoots( query );
        }

        /// <summary>
        /// 是否加载子节点
        /// </summary>
        private bool IsLoadChilds() {
            return GetOperation() == "LoadChilds";
        }

        /// <summary>
        /// 获取查询操作符
        /// </summary>
        private string GetOperation() {
            return Request["QueryOperation"];
        }

        /// <summary>
        /// 获取Id参数
        /// </summary>
        private string GetId() {
            return Request["Id"];
        }

        /// <summary>
        /// 获取下级子节点
        /// </summary>
        private ActionResult GetChildsResult() {
            var query = new TQuery();
            var result = GetChilds( query );
            return ToTreeGridResult( result, LoadMode != LoadMode.RootAsync );
        }

        /// <summary>
        /// 获取下级子节点
        /// </summary>
        private IEnumerable<TDto> GetChilds( TQuery query ) {
            SetPageBySync( query );
            FilterByGetChilds( query );
            List<TDto> result = Service.Query( query );
            result.RemoveAll( t => GetId( t ) == GetId() );
            return result;
        }

        /// <summary>
        /// 设置同步分页条件
        /// </summary>
        private void SetPageBySync( TQuery query ) {
            query.Page = 1;
            query.PageSize = 1000;
            query.Order = "SortId";
        }

        /// <summary>
        /// 过滤下级子节点
        /// </summary>
        private void FilterByGetChilds( TQuery query ) {
            if ( LoadMode == LoadMode.Async ) {
                query.ParentId = GetId().ToGuid();
                return;
            }
            var dto = Service.Get( GetId() ) ?? new TDto();
            query.Path = dto.Path;
        }

        /// <summary>
        /// 获取Dto的Id
        /// </summary>
        private string GetId( TDto dto ) {
            return ( (IDto)dto ).Id;
        }

        /// <summary>
        /// 是否查询
        /// </summary>
        private bool IsQuery( TQuery query ) {
            return GetOperation() == "Query" && query.IsEmpty() == false;
        }

        /// <summary>
        /// 获取根节点列表
        /// </summary>
        protected virtual ActionResult GetRoots( TQuery query ) {
            FilterRoots( query );
            var result = Service.Query( query );
            return ToTreeGridResult( result, LoadMode != LoadMode.Sync, result.TotalCount );
        }

        /// <summary>
        /// 过滤根节点
        /// </summary>
        private void FilterRoots( TQuery query ) {
            if ( LoadMode == LoadMode.Sync ) {
                SetPageBySync( query );
                return;
            }
            SetPage( query );
            query.Level = 1;
        }

        /// <summary>
        /// 获取查询结果
        /// </summary>
        private ActionResult GetQueryResult( TQuery query ) {
            SetPage( query );
            var result = Service.Query( query );
            AddParents( result );
            return ToTreeGridResult( result, true, result.TotalCount );
        }

        /// <summary>
        /// 添加全部父节点
        /// </summary>
        private void AddParents( List<TDto> result ) {
            var ids = new List<string>();
            foreach ( var dto in result )
                AddParentIds( ids, dto );
            foreach ( var dto in result ) {
                if ( ids.Contains( GetId( dto ) ) )
                    ids.Remove( GetId( dto ) );
            }
            var list = Service.GetByIds( ids.Splice() );
            result.AddRange( list );
        }

        /// <summary>
        /// 添加父节点编号
        /// </summary>
        private void AddParentIds( List<string> ids, TDto dto ) {
            var idList = Service.GetParentIdsFromPath( dto );
            idList.ForEach( id => {
                if ( !ids.Contains( id ) )
                    ids.Add( id );
            } );
        }

        #endregion

        #region Enable(启用)

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="ids">Id列表</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormExceptionHandler]
        [AjaxOnly]
        public ActionResult Enable( string ids ) {
            var data = Service.Enable( ids );
            return Ok( "启用成功", data );
        }

        #endregion

        #region Disable(冻结)

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="ids">Id列表</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormExceptionHandler]
        [AjaxOnly]
        public ActionResult Disable( string ids ) {
            var data = Service.Disable( ids );
            return Ok( "冻结成功", data );
        }

        #endregion
    }
}
