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
    /// 字典服务
    /// </summary>
    public class DicService : TreeBatchService<Dic, DicDto, DicQuery>, IDicService {

        #region 构造方法

        /// <summary>
        /// 初始化字典服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">字典仓储</param>
        public DicService(IBeiDreamAppUnitOfWork unitOfWork, IDicRepository repository)
            : base( unitOfWork, repository ) {
            Repository = repository;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 字典仓储
        /// </summary>
        protected new IDicRepository Repository { get; set; }

        #endregion

        #region 实体与Dto转换

        /// <summary>
        /// 转换为数据传输对象
        /// </summary>
        /// <param name="entity">实体</param>
        protected override DicDto ToDto( Dic entity ) {
            return entity.ToDto();
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        protected override Dic ToEntity( DicDto dto ) {
            return dto.ToEntity();
        }

        #endregion

        #region GetQuery(查询)

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="query">字典查询参数</param>
        protected override IQueryBase<Dic> GetQuery( DicQuery query ) {
            return new Query<Dic>(query).Filter(new TreeEntityCriteria<Dic>(query))
                .Filter( t => t.Code == query.Code )
                .Filter( t => t.Text.Contains( query.Text ) )
                .Filter( t => t.PinYin.Contains( query.Text ), true )
                .FilterDate( t => t.CreateTime, query.BeginCreateTime, query.EndCreateTime );
        }

        #endregion

        #region Create(创建新实体)

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <param name="parentId">父字典标识</param>
        public override DicDto Create( string parentId ) {
            var result = base.Create( parentId );
            result.Enabled = true;
            result.CreateTime = DateTime.Now;
            return result;
        }

        #endregion

        #region SaveBefore(保存前操作)

        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="addList">新增列表</param>
        /// <param name="updateList">修改列表</param>
        /// <param name="deleteList">删除列表</param>
        protected override void SaveBefore( List<DicDto> addList, List<DicDto> updateList, List<DicDto> deleteList ) {
            base.SaveBefore( addList, updateList, deleteList );
            Validate( addList );
            Validate( updateList );
        }

        /// <summary>
        /// 验证
        /// </summary>
        private void Validate( IEnumerable<DicDto> list ) {
            foreach ( var each in list.GroupBy( t => t.Code ) ) {
                if ( each.Count( t => t.Code.IsEmpty() == false ) > 1 )
                    ThrowRepeatException( each.Key );
            }
        }

        /// <summary>
        /// 抛出编码重复异常
        /// </summary>
        private void ThrowRepeatException( string code ) {
            throw new Warning(string.Format("字典编码 '{0}' 已存在，请修改", code));
        }

        #endregion

        #region AddBefore(添加前操作)

        /// <summary>
        /// 添加前操作
        /// </summary>
        /// <param name="entity">字典实体</param>
        protected override void AddBefore( Dic entity ) {
            base.AddBefore( entity );
            var exists = Repository.Exists( t => t.TenantId == entity.TenantId && t.Code != "" && t.Code == entity.Code );
            if ( exists )
                ThrowRepeatException( entity.Code );
        }

        #endregion

        #region UpdateBefore(修改前操作)

        /// <summary>
        /// 修改前操作
        /// </summary>
        /// <param name="newEntity">新实体</param>
        /// <param name="oldEntity">数据库中的旧实体</param>
        protected override void UpdateBefore( Dic newEntity, Dic oldEntity ) {
            base.UpdateBefore( newEntity, oldEntity );
            ValidateUpdateExists( newEntity );
            newEntity.Init();
        }

        /// <summary>
        /// 验证更新时是否存在重复编码
        /// </summary>
        private void ValidateUpdateExists( Dic entity ) {
            var exists = Repository.Exists( t => t.TenantId == entity.TenantId && t.Id != entity.Id && t.Code != "" && t.Code == entity.Code );
            if ( exists )
                ThrowRepeatException( entity.Code );
        }

        #endregion

        #region Load(根据编码加载字典列表)

        /// <summary>
        /// 根据编码加载字典列表
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <param name="tenantId">租户编号</param>
        public List<DicDto> Load( string code, Guid? tenantId = null ) {
            var entity = Repository.GetDic( code, tenantId );
            if ( entity == null )
                return new List<DicDto>();
            var query = new DicQuery { PageSize = 1000, Path = entity.Path, Enabled = true };
            return Query( query ).Where( t => t.Id != entity.Id.ToString() ).ToList();
        }

        #endregion
    }
}