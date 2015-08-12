using System;
using System.ComponentModel.DataAnnotations;

namespace Util.Domains {
    /// <summary>
    /// 树型实体查询参数
    /// </summary>
    /// <typeparam name="TParentId">父编号类型</typeparam>
    public interface ITreeEntityQuery<TParentId> {
        /// <summary>
        /// 父编号
        /// </summary>
        TParentId ParentId { get; set; }
        /// <summary>
        /// 级数
        /// </summary>
        int? Level { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [Display( Name = "启用" )]
        bool? Enabled { get; set; }
        /// <summary>
        /// 查询参数是否全部为空
        /// </summary>
        bool IsEmpty();
    }

    /// <summary>
    /// 树型实体查询参数
    /// </summary>
    public interface ITreeEntityQuery : ITreeEntityQuery<Guid?> {
    }
}
