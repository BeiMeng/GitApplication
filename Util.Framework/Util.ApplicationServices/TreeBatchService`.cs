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
    public abstract class TreeBatchService<TEntity, TDto, TQuery> : TreeBatchService<TEntity, TDto, TQuery, Guid, Guid?>
        where TEntity : class, IAggregateRoot<TEntity, Guid>, ITreeEntity<TEntity, Guid, Guid?>, new()
        where TDto : IDto, new()
        where TQuery : IPager, ITreeEntityQuery {
        /// <summary>
        /// 初始化批操作服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected TreeBatchService( IUnitOfWork unitOfWork, IRepository<TEntity, Guid> repository )
            : base( unitOfWork, repository ) {
        }

        /// <summary>
        /// 获取排序号
        /// </summary>
        /// <param name="parentId">父Id</param>
        protected override int GetSortId( Guid? parentId ) {
            var maxSortId = Repository.Find().Where( t => t.ParentId == parentId ).Max( t => t.SortId );
            return maxSortId.SafeValue() + 1;
        }

        /// <summary>
        /// 获取直接下级
        /// </summary>
        /// <param name="parent">父实体</param>
        protected override List<TEntity> GetChilds( TEntity parent ) {
            return Repository.Find().Where( t => t.ParentId == parent.Id ).ToList();
        }

        /// <summary>
        /// 获取根节点
        /// </summary>
        protected override List<TEntity> GetRoots() {
            return Repository.Find().Where( t => t.ParentId == null ).ToList();
        }
    }
}
