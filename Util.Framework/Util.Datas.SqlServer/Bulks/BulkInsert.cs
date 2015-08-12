using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using EntityFramework.Mapping;
using Util.Datas.Ef;

namespace Util.Datas.SqlServer.Bulks {
    /// <summary>
    /// 批量插入
    /// </summary>
    public class BulkInsert<TEntity> where TEntity : class {
        /// <summary>
        /// 初始化批量插入
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="entities">实体集</param>
        public BulkInsert( DbContext context, IEnumerable<TEntity> entities ) {
            Context = context;
            Entities = entities;
            Map = Context.GetMetaData<TEntity>();
        }

        /// <summary>
        /// 数据上下文
        /// </summary>
        protected DbContext Context { get; set; }

        /// <summary>
        /// 实体集
        /// </summary>
        protected IEnumerable<TEntity> Entities { get; set; }

        /// <summary>
        /// 实体映射
        /// </summary>
        protected EntityMap Map { get; set; }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="batchSize">批大小</param>
        /// <param name="options">批量选项</param>
        public int Insert( int batchSize = 1000, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default ) {
            var dt = Ef.Helper.ToDataTable( Entities.ToList(), Map );
            return SqlHelper.BulkCopy( (SqlConnection)Context.Database.Connection, Map.TableName, dt, batchSize, options );
        }
    }
}
