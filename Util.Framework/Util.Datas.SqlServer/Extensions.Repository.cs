using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Dappers;
using Util.Datas.Ef;
using Util.Datas.SqlServer.Bulks;
using Util.Domains;
using Util.Domains.Repositories;

namespace Util.Datas.SqlServer {
    /// <summary>
    /// 仓储扩展
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体标识类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="entities">实体集合</param>
        /// <param name="batchSize">批大小</param>
        /// <param name="options">批量选项</param>
        public static int BulkInsert<TEntity, TKey>( this IRepository<TEntity, TKey> repository, IEnumerable<TEntity> entities, int batchSize = 1000, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default ) where TEntity : class,IAggregateRoot<TEntity, TKey> {
            return new BulkInsert<TEntity>( (DbContext)repository.GetUnitOfWork(), entities ).Insert( batchSize, options );
        }

        /// <summary>
        /// 通过Sql执行数据更新操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体标识类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">Sql参数，使用匿名对象传入，范例：new{Name="A"}</param>
        /// <param name="transaction">事务</param>
        public static int Execute<TEntity, TKey>( this IRepository<TEntity, TKey> repository, string sql, object param = null, IDbTransaction transaction = null ) where TEntity : class, IAggregateRoot<TEntity, TKey> {
            var context = (DbContext)repository.GetUnitOfWork();
            return context.Database.Connection.Execute( sql, param, transaction );
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体标识类型</typeparam>
        /// <param name="repository">仓储</param>
        public static void Truncate<TEntity, TKey>( this IRepository<TEntity, TKey> repository ) where TEntity : class, IAggregateRoot<TEntity, TKey> {
            var metadata = repository.GetMetaData();
            string sql = string.Format( "Truncate Table {0}",metadata.TableName );
            repository.Execute( sql );
        }
    }
}
