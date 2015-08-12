using System;
using System.ComponentModel.DataAnnotations;
using Util.Validations;

namespace Util.Domains {
    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TKey">标识类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class AggregateRoot<TEntity, TKey> : EntityBase<TEntity, TKey>, IAggregateRoot<TEntity, TKey> where TEntity : IAggregateRoot {
        /// <summary>
        /// 初始化聚合根
        /// </summary>
        /// <param name="id">标识</param>
        protected AggregateRoot( TKey id )
            : base( id ) {
        }

        /// <summary>
        /// 版本号(乐观锁)
        /// </summary>
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class AggregateRoot<TEntity> : AggregateRoot<TEntity, Guid> where TEntity : IAggregateRoot {
        /// <summary>
        /// 初始化聚合根
        /// </summary>
        /// <param name="id">标识</param>
        protected AggregateRoot( Guid id )
            : base( id ) {
        }

        /// <summary>
        /// 验证
        /// </summary>
        protected override void Validate( ValidationResultCollection results ) {
            if ( Id == Guid.Empty )
                results.Add( new ValidationResult( "Id不能为空" ) );
        }
    }
}


