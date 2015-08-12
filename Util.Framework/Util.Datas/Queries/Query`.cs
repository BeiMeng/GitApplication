using System;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.Queries {
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class Query<TEntity> : Query<TEntity, Guid>, IQuery<TEntity> where TEntity : class ,IAggregateRoot<TEntity, Guid> {
        /// <summary>
        /// 初始化查询对象
        /// </summary>
        public Query() {
        }

        /// <summary>
        /// 初始化查询对象
        /// </summary>
        /// <param name="pager">分页对象</param>
        public Query( IPager pager ) : base( pager ) {
        }
    }
}
