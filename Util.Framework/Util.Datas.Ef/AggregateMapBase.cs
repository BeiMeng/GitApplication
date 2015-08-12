using System;
using System.ComponentModel.DataAnnotations.Schema;
using Util.Domains;

namespace Util.Datas.Ef {
    /// <summary>
    /// 聚合根映射
    /// </summary>
    /// <typeparam name="TEntity">聚合根类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class AggregateMapBase<TEntity, TKey> : EntityMapBase<TEntity> where TEntity : class, IAggregateRoot<TEntity, TKey> {
        /// <summary>
        /// 映射乐观离线锁
        /// </summary>
        protected override void MapVersion() {
            if ( IsSupportRowVersion ) {
                MapRowVersion();
                return;
            }
            MapConcurrencyToken();
        }

        /// <summary>
        /// 是否支持行版本，Sql Server支持，MySql等不支持
        /// </summary>
        protected virtual bool IsSupportRowVersion {
            get { return true; }
        }

        /// <summary>
        /// 映射行版本
        /// </summary>
        private void MapRowVersion() {
            Property( t => t.Version ).HasColumnName( "Version" ).IsRowVersion().HasDatabaseGeneratedOption( DatabaseGeneratedOption.Computed ).IsOptional();
        }

        /// <summary>
        /// 映射乐观并发标记
        /// </summary>
        private void MapConcurrencyToken() {
            Property( t => t.Version ).HasColumnName( "Version" ).IsConcurrencyToken().HasDatabaseGeneratedOption( DatabaseGeneratedOption.None ).IsOptional();
        }
    }

    /// <summary>
    /// 聚合根映射
    /// </summary>
    /// <typeparam name="TEntity">聚合根类型</typeparam>
    public abstract class AggregateMapBase<TEntity> : AggregateMapBase<TEntity, Guid> where TEntity : class ,IAggregateRoot<TEntity, Guid> {
    }
}
