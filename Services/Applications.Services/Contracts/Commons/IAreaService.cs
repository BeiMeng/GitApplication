using System;
using System.Collections.Generic;
using Applications.Domains.Queries.Commons;
using Applications.Services.Dtos.Commons;
using Util.ApplicationServices;

namespace Applications.Services.Contracts.Commons {
    /// <summary>
    /// 地区服务
    /// </summary>
    public interface IAreaService : ITreeBatchService<AreaDto, AreaQuery> {
        /// <summary>
        /// 加载地区数据
        /// </summary>
        /// <param name="parentId">父编号</param>
        List<AreaDto> Load( Guid? parentId );
    }
}