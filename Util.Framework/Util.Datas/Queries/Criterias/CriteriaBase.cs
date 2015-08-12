using System;
using System.Linq.Expressions;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.Queries.Criterias {
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class CriteriaBase<TEntity> : ICriteria<TEntity> where TEntity : class, IAggregateRoot {
        /// <summary>
        /// 谓词
        /// </summary>
        protected Expression<Func<TEntity, bool>> Predicate { get; set; }

        /// <summary>
        /// 获取谓词
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> GetPredicate() {
            return Predicate;
        }
    }
}
