﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Util.Domains;
using Util.Domains.Repositories;
using Util.Security;

namespace Util.ApplicationServices.Criterias.Permissions {
    /// <summary>
    /// 权限条件管理器
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class PermissionCriteriaManagerBase<T> : ICriteria<T> where T : class, IAggregateRoot {
        /// <summary>
        /// 初始化权限条件管理器
        /// </summary>
        /// <param name="permissionManager">权限管理器</param>
        protected PermissionCriteriaManagerBase( IPermissionManager permissionManager ) {
            PermissionManager = permissionManager;
            _criterias = new List<ICriteria<T>>();
        }

        /// <summary>
        /// 谓词
        /// </summary>
        private Expression<Func<T, bool>> Predicate { get; set; }

        /// <summary>
        /// 权限管理器
        /// </summary>
        protected IPermissionManager PermissionManager { get; set; }

        /// <summary>
        /// 查询条件集合
        /// </summary>
        private readonly ICollection<ICriteria<T>> _criterias;

        /// <summary>
        /// 获取谓词
        /// </summary>
        public Expression<Func<T, bool>> GetPredicate() {
            Limit();
            AddCriterias( _criterias );
            return Predicate.Or( GeneratePredicate() );
        }

        /// <summary>
        /// 限制数据访问
        /// </summary>
        private void Limit() {
            Predicate = t => false;
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="criterias">查询条件集合</param>
        protected abstract void AddCriterias( ICollection<ICriteria<T>> criterias );

        /// <summary>
        /// 生成谓词
        /// </summary>
        private Expression<Func<T, bool>> GeneratePredicate() {
            Expression<Func<T, bool>> predicate = null;
            foreach( var criteria in _criterias )
                predicate = predicate.And( criteria.GetPredicate() );
            return predicate;
        }
    }
}