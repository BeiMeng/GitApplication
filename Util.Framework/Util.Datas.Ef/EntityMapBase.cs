using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using Util.Domains;

namespace Util.Datas.Ef {
    /// <summary>
    /// 实体映射
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class EntityMapBase<TEntity> : EntityTypeConfiguration<TEntity>, IMap where TEntity : class, IEntity {
        /// <summary>
        /// 初始化映射
        /// </summary>
        protected EntityMapBase() {
            MapTable();
            MapId();
            MapVersion();
            MapProperties();
            MapAssociations();
        }

        /// <summary>
        /// 映射表
        /// </summary>
        protected abstract void MapTable();

        /// <summary>
        /// 映射标识
        /// </summary>
        protected virtual void MapId() {
        }

        /// <summary>
        /// 映射乐观离线锁
        /// </summary>
        protected virtual void MapVersion() {
        }

        /// <summary>
        /// 映射属性
        /// </summary>
        protected virtual void MapProperties() {
        }

        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected virtual void MapAssociations() {
        }

        /// <summary>
        /// 将配置添加到管理器
        /// </summary>
        /// <param name="registrar">配置管理器</param>
        public void AddTo( ConfigurationRegistrar registrar ) {
            registrar.Add( this );
        }
    }
}
