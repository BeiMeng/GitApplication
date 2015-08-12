using System.Data.Entity.ModelConfiguration.Configuration;

namespace Util.Datas.Ef {
    /// <summary>
    /// 映射
    /// </summary>
    public interface IMap {
        /// <summary>
        /// 将配置添加到管理器
        /// </summary>
        /// <param name="registrar">配置管理器</param>
        void AddTo( ConfigurationRegistrar registrar );
    }
}
