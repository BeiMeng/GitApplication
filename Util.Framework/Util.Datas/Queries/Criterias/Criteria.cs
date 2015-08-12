using System;
using System.Linq.Expressions;
using Util.Domains;

namespace Util.Datas.Queries.Criterias {
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    internal class Criteria<TEntity> : CriteriaBase<TEntity> where TEntity : class,IAggregateRoot {
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <param name="predicate">谓词</param>
        public Criteria( Expression<Func<TEntity, bool>> predicate ) {
            Predicate = predicate;
        }
    }
}
