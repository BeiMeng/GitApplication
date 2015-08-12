using System;
using Util.Domains;

namespace Util.Datas.Queries {
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IQuery<TEntity> : IQuery<TEntity, Guid> where TEntity : class,IAggregateRoot<TEntity, Guid> {
    }
}
