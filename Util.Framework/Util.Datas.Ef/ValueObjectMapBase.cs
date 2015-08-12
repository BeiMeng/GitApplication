using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Util.Datas.Ef {
    /// <summary>
    /// 值对象映射
    /// </summary>
    /// <typeparam name="TValueObject">值对象类型</typeparam>
    public abstract class ValueObjectMapBase<TValueObject> : ComplexTypeConfiguration<TValueObject>, IMap where TValueObject : class {
        /// <summary>
        /// 初始化值对象映射
        /// </summary>
        protected ValueObjectMapBase() {
            MapProperties();
        }

        /// <summary>
        /// 映射属性
        /// </summary>
        protected abstract void MapProperties();

        /// <summary>
        /// 将配置添加到管理器
        /// </summary>
        /// <param name="registrar">配置管理器</param>
        public void AddTo( ConfigurationRegistrar registrar ) {
            registrar.Add( this );
        }
    }
}
