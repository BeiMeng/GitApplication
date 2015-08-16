using System;
using Applications.Domains.Models.Commons;
using Applications.Domains.Repositories.Commons;
using Infrastructure.Core;

namespace Datas.Ef.Repositories.Commons {
    /// <summary>
    /// 字典仓储
    /// </summary>
    public class DicRepository : RepositoryBase<Dic>, IDicRepository {
        /// <summary>
        /// 初始化字典仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public DicRepository(IBeiDreamAppUnitOfWork unitOfWork)
            : base( unitOfWork ) {
        }

        /// <summary>
        /// 根据编码获取字典
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <param name="tenantId">租户编号</param>
        public Dic GetDic( string code, Guid? tenantId = null ) {
            var result = Single( t => t.Code == code && t.TenantId == tenantId );
            if ( result != null )
                return result;
            return Single( t => t.Code == code && t.TenantId == null );
        }
    }
}
