using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text;
using Util.Datas.Ef;
using Util.Domains;

namespace Util.Datas.MySql {
    /// <summary>
    /// MySql工作单元
    /// </summary>
    public abstract class MySqlUnitOfWork : EfUnitOfWork {
        /// <summary>
        /// 初始化MySql工作单元
        /// </summary>
        /// <param name="connectionName">连接字符串的名称</param>
        protected MySqlUnitOfWork( string connectionName ) : base( connectionName ){
        }

        /// <summary>
        /// 保存更新
        /// </summary>
        public override int SaveChanges() {
            var objectContext = ( (IObjectContextAdapter)this ).ObjectContext;
            foreach( ObjectStateEntry entry in objectContext.ObjectStateManager.GetObjectStateEntries( EntityState.Modified | EntityState.Added ) )
                InitVersion( entry );
            return base.SaveChanges();
        }

        /// <summary>
        /// 初始化版本号
        /// </summary>
        private void InitVersion( ObjectStateEntry entry ) {
            var entity = entry.Entity as IAggregateRoot;
            if ( entity == null )
                return;
            entity.Version = Encoding.UTF8.GetBytes( Guid.NewGuid().ToString() );
        }
    }
}
