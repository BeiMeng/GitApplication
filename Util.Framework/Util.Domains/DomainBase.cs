﻿using System.Collections.Generic;
using System.Linq;
using Util.Validations;

namespace Util.Domains {
    /// <summary>
    /// 领域层顶级基类
    /// </summary>
    public abstract class DomainBase<T> : StateDescription, INullObject, IDomainObject, ICompareChange<T> where T : IDomainObject {

        #region 构造方法

        /// <summary>
        /// 初始化领域层顶级基类
        /// </summary>
        protected DomainBase() {
            _rules = new List<IValidationRule>();
            _handler = new ValidationHandler();
        }

        #endregion

        #region 字段

        /// <summary>
        /// 验证规则集合
        /// </summary>
        private readonly List<IValidationRule> _rules;
        /// <summary>
        /// 验证处理器
        /// </summary>
        private IValidationHandler _handler;
        /// <summary>
        /// 变更值集合
        /// </summary>
        private ChangeValueCollection _changeValues;

        #endregion

        #region SetValidationHandler(设置验证处理器)

        /// <summary>
        /// 设置验证处理器
        /// </summary>
        /// <param name="handler">验证处理器</param>
        public void SetValidationHandler( IValidationHandler handler ) {
            if ( handler == null )
                return;
            _handler = handler;
        }

        #endregion

        #region AddValidationRules(添加验证规则列表)

        /// <summary>
        /// 添加验证规则列表
        /// </summary>
        /// <param name="rules">验证规则列表</param>
        public void AddValidationRules( IEnumerable<IValidationRule> rules ) {
            if ( rules == null )
                return;
            foreach ( var rule in rules )
                AddValidationRule( rule );
        }

        #endregion

        #region AddValidationRule(添加验证规则)

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        public void AddValidationRule( IValidationRule rule ) {
            if ( rule == null )
                return;
            _rules.Add( rule );
        }

        #endregion

        #region Validate(验证)

        /// <summary>
        /// 验证
        /// </summary>
        public virtual void Validate() {
            var result = GetValidationResult();
            HandleValidationResult( result );
        }

        /// <summary>
        /// 获取验证结果
        /// </summary>
        private ValidationResultCollection GetValidationResult() {
            var result = ValidationFactory.Create().Validate( this );
            Validate( result );
            foreach ( var rule in _rules )
                result.Add( rule.Validate() );
            return result;
        }

        /// <summary>
        /// 验证并添加到验证结果集合
        /// </summary>
        /// <param name="results">验证结果集合</param>
        protected virtual void Validate( ValidationResultCollection results ) {
        }

        /// <summary>
        /// 处理验证结果
        /// </summary>
        private void HandleValidationResult( ValidationResultCollection results ) {
            if ( results.IsValid )
                return;
            _handler.Handle( results );
        }

        #endregion

        #region IsNull(是否空对象)

        /// <summary>
        /// 是否空对象
        /// </summary>
        public virtual bool IsNull() {
            return false;
        }

        #endregion

        #region GetChanges(获取变更属性)

        /// <summary>
        /// 获取变更属性
        /// </summary>
        /// <param name="newEntity">新对象</param>
        public ChangeValueCollection GetChanges( T newEntity ) {
            if( Equals( newEntity, null ) )
                return new ChangeValueCollection();
            _changeValues = new ChangeValueCollection();
            AddChanges( newEntity );
            return _changeValues;
        }

        /// <summary>
        /// 添加变更列表
        /// </summary>
        /// <param name="newEntity">新对象</param>
        protected virtual void AddChanges( T newEntity ) {
        }

        /// <summary>
        /// 添加变更
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="description">描述</param>
        /// <param name="oldValue">旧值,范例：this.Name</param>
        /// <param name="newValue">新值,范例：newEntity.Name</param>
        /// <param name="isAttention">是否关注</param>
        protected void AddChange<TValue>( string propertyName, string description, TValue oldValue, TValue newValue,bool isAttention = false ) {
            if ( Equals( oldValue, newValue ) )
                return;
            string oldValueString = oldValue.ToStr().ToLower().Trim();
            string newValueString = newValue.ToStr().ToLower().Trim();
            if ( oldValueString == newValueString )
                return;
            _changeValues.Add( propertyName, description, oldValueString, newValueString, isAttention );
        }

        /// <summary>
        /// 添加变更
        /// </summary>
        /// <param name="oldObject">旧对象</param>
        /// <param name="newObject">新对象</param>
        protected void AddChange<TDomainObject>( ICompareChange<TDomainObject> oldObject, TDomainObject newObject ) where TDomainObject : IDomainObject {
            if ( Equals( oldObject, null ) )
                return;
            if ( Equals( newObject, null ) )
                return;
            _changeValues.AddRange( oldObject.GetChanges( newObject ) );
        }

        /// <summary>
        /// 添加变更
        /// </summary>
        /// <param name="oldObjects">旧对象列表</param>
        /// <param name="newObjects">新对象列表</param>
        protected void AddChange<TDomainObject>( IEnumerable<ICompareChange<TDomainObject>> oldObjects, IEnumerable<TDomainObject> newObjects ) where TDomainObject : IDomainObject {
            if ( Equals( oldObjects, null ) )
                return;
            if ( Equals( newObjects, null ) )
                return;
            var oldList = oldObjects.ToList();
            var newList = newObjects.ToList();
            for ( int i = 0; i < oldList.Count; i++ ) {
                if( newList.Count <= i )
                    return;
                AddChange( oldList[i], newList[i] );
            }
        }

        #endregion
    }
}
