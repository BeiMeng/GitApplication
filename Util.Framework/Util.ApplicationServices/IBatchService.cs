using System.Collections.Generic;

namespace Util.ApplicationServices {
    /// <summary>
    /// 批操作服务
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询实体类型</typeparam>
    public interface IBatchService<TDto, in TQuery> : IServiceBase<TDto, TQuery> where TDto : new() {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="addList">新增列表</param>
        /// <param name="updateList">修改列表</param>
        /// <param name="deleteList">删除列表</param>
        List<TDto> Save( List<TDto> addList, List<TDto> updateList, List<TDto> deleteList );
    }
}
