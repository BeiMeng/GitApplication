using System;
using System.Collections.Generic;
using System.Linq;
using Util.Domains;

namespace Util.Datas.Ef {
    /// <summary>
    /// 实体更改分组
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">标识类型</typeparam>
    public class ChangeGrouping<TEntity, TKey> where TEntity : IEntity<TEntity, TKey> {
        /// <summary>
        /// 初始化实体更改分组
        /// </summary>
        /// <param name="entities">实体集合，一般由表示层传入</param>
        /// <param name="dbEntities">数据库中对应的实体集合</param>
        public ChangeGrouping( IEnumerable<TEntity> entities, IEnumerable<TEntity> dbEntities ) {
            Entities = entities.ToList();
            DbEntities = dbEntities.ToList();
        }

        /// <summary>
        /// 实体集合
        /// </summary>
        private List<TEntity> Entities { get; set; }

        /// <summary>
        /// 数据库中对应的实体集合
        /// </summary>
        private List<TEntity> DbEntities { get; set; }

        /// <summary>
        /// 获取新增的实体集
        /// </summary>
        public IEnumerable<TEntity> GetNewEntities() {
            return Entities.Where( entity => !DbEntities.Contains( entity ) ).ToList();
        }

        /// <summary>
        /// 获取被修改的实体集
        /// </summary>
        public IEnumerable<TEntity> GetUpdateEntities() {
            return Entities.Where( entity => DbEntities.Contains( entity ) ).ToList();
        }

        /// <summary>
        /// 获取被删除的实体集
        /// </summary>
        public IEnumerable<TEntity> GetDeleteEntities() {
            return DbEntities.Where( entity => !Entities.Contains( entity ) ).ToList();
        }
    }

    /// <summary>
    /// 实体更改分组
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class ChangeGrouping<TEntity> : ChangeGrouping<TEntity, Guid> where TEntity : IEntity<TEntity, Guid> {
        /// <summary>
        /// 初始化实体更改分组
        /// </summary>
        /// <param name="entities">实体集合，一般由表示层传入</param>
        /// <param name="dbEntities">数据库中对应的实体集合</param>
        public ChangeGrouping( IEnumerable<TEntity> entities, IEnumerable<TEntity> dbEntities )
            : base( entities, dbEntities ) {
        }
    }
}
