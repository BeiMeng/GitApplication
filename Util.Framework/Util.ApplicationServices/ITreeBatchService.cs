using System.Collections.Generic;
using Util.Domains.Repositories;

namespace Util.ApplicationServices {
    /// <summary>
    /// 树型实体批操作服务
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    public interface ITreeBatchService<TDto, in TQuery> : IBatchService<TDto, TQuery>
        where TDto : IDto, new()
        where TQuery : IPager {
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="parentId">父Id</param>
        TDto Create( string parentId );
        /// <summary>
        /// 从路径中获取所有上级节点编号
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        List<string> GetParentIdsFromPath( TDto dto );
        /// <summary>
        /// 启用字典
        /// </summary>
        /// <param name="ids">字典编号列表</param>
        List<TDto> Enable( string ids );
        /// <summary>
        /// 冻结字典
        /// </summary>
        /// <param name="ids">字典编号列表</param>
        List<TDto> Disable( string ids );
        /// <summary>
        /// 修正路径
        /// </summary>
        void FixPath();
    }
}
