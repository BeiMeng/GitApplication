using System;
using System.Collections.Generic;
using System.Linq;
using Applications.Domains.Models.Commons;
using Applications.Domains.Queries.Commons;
using Applications.Domains.Repositories.Commons;
using Applications.Services.Contracts.Commons;
using Applications.Services.Dtos.Commons;
using Infrastructure.Core;
using Util;
using Util.ApplicationServices;
using Util.ApplicationServices.Criterias;
using Util.Datas.Queries;
using Util.Domains.Repositories;

namespace Applications.Services.Impl.Commons {
    /// <summary>
    /// 地区服务
    /// </summary>
    public class AreaService : TreeBatchService<Area, AreaDto, AreaQuery>, IAreaService {

        #region 构造方法

        /// <summary>
        /// 初始化地区服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">地区仓储</param>
        public AreaService(IBeiDreamAppUnitOfWork unitOfWork, IAreaRepository repository)
            : base( unitOfWork, repository ) {
            Repository = repository;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 地区仓储
        /// </summary>
        protected new IAreaRepository Repository { get; set; }

        #endregion

        #region 实体与Dto转换

        /// <summary>
        /// 转换为数据传输对象
        /// </summary>
        /// <param name="entity">实体</param>
        protected override AreaDto ToDto( Area entity ) {
            return entity.ToDto();
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override Area ToEntity( AreaDto dto ) {
            return dto.ToEntity();
        }

        #endregion

        #region Create(创建新实体)

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <param name="parentId">父字典标识</param>
        public override AreaDto Create( string parentId ) {
            var result = base.Create( parentId );
            result.Enabled = true;
            result.CreateTime = DateTime.Now;
            return result;
        }

        #endregion

        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">地区查询参数</param>
        protected override IQueryBase<Area> GetQuery(AreaQuery query)
        {
            return new Query<Area>( query ).Filter( new TreeEntityCriteria<Area>( query ) )
                .Filter( t => t.Text.Contains( query.Text ) )
                .Filter( t => t.PinYin.StartsWith( query.Text ), true )
                .Filter( t => t.FullPinYin.StartsWith( query.Text ), true )
                .FilterDate( t => t.CreateTime, query.BeginCreateTime, query.EndCreateTime );
        }

        #endregion

        #region SaveBefore(保存前操作)

        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="addList">新增列表</param>
        /// <param name="updateList">修改列表</param>
        /// <param name="deleteList">删除列表</param>
        protected override void SaveBefore( List<AreaDto> addList, List<AreaDto> updateList, List<AreaDto> deleteList ) {
            base.SaveBefore( addList, updateList, deleteList );
            Validate( addList );
            Validate( updateList );
        }

        /// <summary>
        /// 验证
        /// </summary>
        private void Validate( IEnumerable<AreaDto> list ) {
            foreach ( var each in list.GroupBy( t => new { t.ParentId, t.Text } ) ) {
                if ( each.Count() > 1 )
                    ThrowRepeatException( each.First().Text );
            }
        }

        /// <summary>
        /// 抛出编码重复异常
        /// </summary>
        private void ThrowRepeatException( string text ) {
            throw new Warning(string.Format("地区 '{0}' 已存在，请修改", text));
        }

        #endregion

        #region AddBefore(添加前操作)

        /// <summary>
        /// 添加前操作
        /// </summary>
        /// <param name="entity">字典实体</param>
        protected override void AddBefore( Area entity ) {
            base.AddBefore( entity );
            var exists = Repository.Exists( t => t.ParentId == entity.ParentId && t.Text == entity.Text );
            if ( exists )
                ThrowRepeatException( entity.Text );
        }

        #endregion

        #region UpdateBefore(修改前操作)

        /// <summary>
        /// 修改前操作
        /// </summary>
        /// <param name="newEntity">新实体</param>
        /// <param name="oldEntity">数据库中的旧实体</param>
        protected override void UpdateBefore( Area newEntity, Area oldEntity ) {
            base.UpdateBefore( newEntity, oldEntity );
            ValidateUpdateExists( newEntity );
            newEntity.Init();
        }

        /// <summary>
        /// 验证更新时是否存在重复地区
        /// </summary>
        private void ValidateUpdateExists( Area entity ) {
            var exists = Repository.Exists( t => t.Id != entity.Id && t.ParentId == entity.ParentId && t.Text == entity.Text );
            if ( exists )
                ThrowRepeatException( entity.Text );
        }

        #endregion

        #region Load(加载地区数据)

        /// <summary>
        /// 加载地区数据
        /// </summary>
        /// <param name="parentId">父编号</param>
        public List<AreaDto> Load( Guid? parentId ) {
            var query = new AreaQuery { PageSize = 1000, Enabled = true };
            if ( parentId == null )
                query.Level = 1;
            else
                query.ParentId = parentId;
            return Query( query );
        }

        #endregion
    }
}