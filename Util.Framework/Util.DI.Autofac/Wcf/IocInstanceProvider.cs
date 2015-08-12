using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Autofac;
using Autofac.Core;

namespace Util.DI.Autofac.Wcf {
    /// <summary>
    /// Wcf依赖注入提供程序
    /// </summary>
    internal class IocInstanceProvider : IInstanceProvider {
        /// <summary>
        /// 初始化Wcf依赖注入提供程序
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="modules">配置模块</param>
        public IocInstanceProvider( Type type, params IModule[] modules ) {
            ServiceType = type;
            _scope = Container.CreateBuilder( modules ).Build();
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        private Type ServiceType { get; set; }

        /// <summary>
        /// 容器
        /// </summary>
        private readonly ILifetimeScope _scope;
        /// <summary>
        /// 临时容器
        /// </summary>
        private ILifetimeScope _beginScope;

        /// <summary>
        /// 获取实例
        /// </summary>
        public object GetInstance( InstanceContext instanceContext ) {
            return GetInstance( instanceContext, null );
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public object GetInstance( InstanceContext instanceContext, Message message ) {
            _beginScope = _scope.BeginLifetimeScope();
            return _beginScope.Resolve( ServiceType );
        }

        /// <summary>
        /// 释放实例
        /// </summary>
        public void ReleaseInstance( InstanceContext instanceContext, object instance ) {
            _beginScope.Dispose();
            GC.SuppressFinalize( this );
            Context.Remove( Config.GetLogContextKey() );
        }
    }
}
