using System;
using Infrastructure.Core;
using Util.Datas.Ef;
using Util.Domains;

namespace Datas.Ef.Repositories {
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class RepositoryBase<TEntity, TKey> : Repository<TEntity, TKey> where TEntity : class, IAggregateRoot<TEntity, TKey>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        protected RepositoryBase(IBeiDreamAppUnitOfWork unitOfWork )
            : base( unitOfWork ) {
            UnitOfWork = (BeiDreamAppUnitOfWork)unitOfWork;
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        protected new BeiDreamAppUnitOfWork UnitOfWork { get; set; }
    }
    
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, Guid> where TEntity : class, IAggregateRoot<TEntity, Guid>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        protected RepositoryBase(IBeiDreamAppUnitOfWork unitOfWork )
            : base( unitOfWork ) {
        }
    }
}