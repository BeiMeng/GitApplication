using System;
using System.ComponentModel.DataAnnotations;
using Util.Logs;

namespace Util.Domains {
    /// <summary>
    /// 领域实体
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">标识类型</typeparam>
    public abstract class EntityBase<TEntity, TKey> : DomainBase<TEntity>, IEntity<TEntity,TKey> where TEntity : IEntity {

        #region 构造方法

        /// <summary>
        /// 初始化领域实体
        /// </summary>
        /// <param name="id">标识</param>
        protected EntityBase( TKey id ) {
            Id = id;
        }

        #endregion

        #region Id(标识)

        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        [Key]
        public TKey Id { get; private set; }

        #endregion

        #region Equals(相等运算)

        /// <summary>
        /// 相等运算
        /// </summary>
        public override bool Equals( object entity ) {
            if ( entity == null )
                return false;
            if ( !( entity is EntityBase<TEntity,TKey> ) )
                return false;
            return this == (EntityBase<TEntity,TKey>)entity;
        }

        #endregion

        #region GetHashCode(获取哈希)

        /// <summary>
        /// 获取哈希
        /// </summary>
        public override int GetHashCode() {
            return ReferenceEquals( Id, null ) ? 0 : Id.GetHashCode();
        }

        #endregion

        #region ==(相等比较)

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        public static bool operator ==( EntityBase<TEntity, TKey> entity1, EntityBase<TEntity, TKey> entity2 ) {
            if ( (object)entity1 == null && (object)entity2 == null )
                return true;
            if ( (object)entity1 == null || (object)entity2 == null )
                return false;
            if ( Equals( entity1.Id, null ) )
                return false;
            if ( entity1.Id.Equals( default( TKey ) ) )
                return false;
            return entity1.Id.Equals( entity2.Id );
        }

        #endregion

        #region !=(不相等比较)

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        public static bool operator !=( EntityBase<TEntity, TKey> entity1, EntityBase<TEntity, TKey> entity2 ) {
            return !( entity1 == entity2 );
        }

        #endregion
        
        #region Init(初始化)

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init() {
            if ( Id.Equals( default( TKey ) ) )
                Id = CreateId();
        }

        /// <summary>
        /// 创建标识
        /// </summary>
        protected virtual TKey CreateId() {
            return Conv.To<TKey>( Guid.NewGuid() );
        }

        #endregion
    }
}
