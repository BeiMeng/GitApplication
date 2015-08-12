using Autofac;
using Applications.Domains.Repositories.Systems;
using Applications.Services.Contracts.Systems;
using Applications.Services.Impl.Systems;
using Datas.Ef;
using Datas.Ef.Repositories.Systems;
using Infrastructure.Core;
using Util.ApplicationServices;
using Util.Datas.Sql.Queries;
using Util.Datas.SqlServer.Queries;
using Util.Exports;
using Util.Files;
using Util.Offices.Npoi;
using Util.Webs.Controls.ValidateCodes;


namespace Applications.Services.Configs.IocConfigs {
    /// <summary>
    /// Autofac依赖注入配置
    /// </summary>
    public class BeiDreamAppConfig : Util.DI.Autofac.ConfigBase {
        /// <summary>
        /// 加载配置
        /// </summary>
        protected override void Load( ContainerBuilder builder ) {
            LoadBase( builder );            
            LoadDomainServices( builder );
            LoadRepositories( builder );
        }

        /// <summary>
        /// 加载基础设施
        /// </summary>
        private void LoadBase( ContainerBuilder builder ) {
            builder.RegisterType<ValidateCode>().As<IValidateCode>().InstancePerLifetimeScope();
            builder.RegisterType<FileManager>().As<IFileManager>().InstancePerLifetimeScope();
            builder.RegisterType<TenantUploadPathStrategy>().As<IUploadPathStrategy>().SingleInstance();
            builder.RegisterType<ExportFactory>().As<IExportFactory>().SingleInstance();
            builder.RegisterType<BeiDreamAppUnitOfWork>().As<IBeiDreamAppUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<SqlServerQuery2012>().As<ISqlQuery>().InstancePerLifetimeScope();
        }        

        /// <summary>
        /// 加载领域服务
        /// </summary>
        private void LoadDomainServices( ContainerBuilder builder ) {
            builder.RegisterType<TenantService>().As<ITenantService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// 加载仓储
        /// </summary>
        private void LoadRepositories( ContainerBuilder builder ) {
            builder.RegisterType<TenantRepository>().As<ITenantRepository>().InstancePerLifetimeScope();

        }
 
    }
}