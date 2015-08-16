using System;
using System.Collections.Generic;
using Applications.Domains.Queries.Commons;
using Applications.Services.Dtos.Commons;
using Util.ApplicationServices;

namespace Applications.Services.Contracts.Commons {
    /// <summary>
    /// 字典服务
    /// </summary>
    public interface IDicService : ITreeBatchService<DicDto, DicQuery> {
        /// <summary>
        /// 根据编码加载字典列表
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <param name="tenantId">租户编号</param>
        List<DicDto> Load( string code,Guid? tenantId = null );
    }
}