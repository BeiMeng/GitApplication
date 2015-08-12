using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Autofac.Core;
using Util.DI.Autofac.Wcf;

namespace Util.DI.Autofac {
    /// <summary>
    /// Wcf依赖注入
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public abstract class WcfIocAttribute : Attribute, IServiceBehavior {
        /// <summary>
        /// 验证
        /// </summary>
        public void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase ) {
        }

        /// <summary>
        /// 添加绑定参数
        /// </summary>
        public void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters ) {
        }

        /// <summary>
        /// 拦截
        /// </summary>
        public void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHost ) {
            foreach ( var channelDispatcher in serviceHost.ChannelDispatchers ) {
                var dispatcher = channelDispatcher as ChannelDispatcher;
                if ( dispatcher == null )
                    continue;
                foreach ( EndpointDispatcher endpointDispatcher in dispatcher.Endpoints ) {
                    if ( endpointDispatcher.IsSystemEndpoint )
                        continue;
                    endpointDispatcher.DispatchRuntime.InstanceProvider = new IocInstanceProvider( serviceDescription.ServiceType, GetConfig() );
                }
            }
        }

        /// <summary>
        /// 获取依赖注入配置
        /// </summary>
        public abstract IModule[] GetConfig();
    }
}
