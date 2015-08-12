using System;
using Util.Domains;

namespace Util.Datas.Ef {
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class Repository<TEntity> : Repository<TEntity, Guid> where TEntity : class, IAggregateRoot<TEntity, Guid> {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        protected Repository( IUnitOfWork unitOfWork )
            : base( unitOfWork ) {
        }
    }
}
