using System;
using System.Linq;

namespace Util.Domains {
    /// <summary>
    /// 值对象
    /// </summary>
    /// <typeparam name="TValueObject">值对象类型</typeparam>
    public abstract class ValueObjectBase<TValueObject> : DomainBase<TValueObject>, IEquatable<TValueObject> where TValueObject : ValueObjectBase<TValueObject> {

        #region Equals(相等性比较)

        /// <summary>
        /// 相等性比较
        /// </summary>
        public bool Equals( TValueObject other ) {
            return this == other;
        }

        /// <summary>
        /// 相等性比较
        /// </summary>
        public override bool Equals( object other ) {
            return Equals( other as TValueObject );
        }

        #endregion

        #region ==(相等性比较)

        /// <summary>
        /// 相等性比较
        /// </summary>
        public static bool operator ==( ValueObjectBase<TValueObject> valueObject1, ValueObjectBase<TValueObject> valueObject2 ) {
            if ( (object) valueObject1 == null && (object) valueObject2 == null )
                return true;
            if ( (object) valueObject1 == null || (object) valueObject2 == null )
                return false;
            if ( valueObject1.GetType() != valueObject2.GetType() )
                return false;
            var properties = valueObject1.GetType().GetProperties();
            return properties.All( property => property.GetValue( valueObject1 ) == property.GetValue( valueObject2 ) );
        }

        #endregion

        #region !=(不相等比较)

        /// <summary>
        /// 不相等比较
        /// </summary>
        public static bool operator !=( ValueObjectBase<TValueObject> valueObject1, ValueObjectBase<TValueObject> valueObject2 ) {
            return !( valueObject1 == valueObject2 );
        }

        #endregion

        #region GetHashCode(获取哈希)

        /// <summary>
        /// 获取哈希
        /// </summary>
        public override int GetHashCode() {
            var properties = GetType().GetProperties();
            return properties.Select( property => property.GetValue( this ) )
                    .Where( value => value != null )
                    .Aggregate( 0, ( current, value ) => current ^ value.GetHashCode() );
        }

        #endregion

        #region Clone(克隆副本)

        /// <summary>
        /// 克隆副本
        /// </summary>
        public virtual TValueObject Clone() {
            return Conv.To<TValueObject>( MemberwiseClone() );
        }

        #endregion
    }
}
