using System;

namespace Util.Domains {
    /// <summary>
    /// 树型实体
    /// </summary>
    /// <typeparam name="TEntity">树型实体类型</typeparam>
    public abstract class TreeEntityBase<TEntity> : TreeEntityBase<TEntity, Guid, Guid?> where TEntity : ITreeEntity<TEntity, Guid, Guid?> {
        /// <summary>
        /// 初始化树型实体
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="path">路径</param>
        /// <param name="level">级数</param>
        protected TreeEntityBase( Guid id, string path, int level )
            : base( id, path ,level ) {
        }
    }
}
