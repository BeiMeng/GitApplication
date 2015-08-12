using System.Collections.Generic;

namespace Util.Webs.Controls {
    /// <summary>
    /// 树节点
    /// </summary>
    public interface ITreeNode {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 父标识
        /// </summary>
        string ParentId { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// 级数
        /// </summary>
        int? Level { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        string State { get; set; }
        /// <summary>
        /// 子节点集合
        /// </summary>
        List<ITreeNode> Children { get; set; }
    }
}
