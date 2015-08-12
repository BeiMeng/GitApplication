using Util.Datas.Queries.Criterias;
using Util.Domains;

namespace Util.ApplicationServices.Criterias {
    /// <summary>
    /// 树型实体查询条件
    /// </summary>
    public class TreeEntityCriteria<T> : CriteriaBase<T> where T : TreeEntityBase<T> {
        /// <summary>
        /// 初始化树型实体查询条件
        /// </summary>
        /// <param name="query">树型实体查询条件</param>
        public TreeEntityCriteria( TreeEntityQuery query ) {
            if ( query.Level != null )
                Predicate = t => t.Level == query.Level;
            if ( !query.Path.IsEmpty() )
                Predicate = Predicate.And( t => t.Path.StartsWith( query.Path ) );
            if ( query.ParentId != null )
                Predicate = Predicate.And( t => t.ParentId == query.ParentId );
            if ( query.Enabled != null )
                Predicate = Predicate.And( t => t.Enabled == query.Enabled );
        }
    }
}
