using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Util;
using Util.ApplicationServices;
using Util.Webs.Controls;

namespace Applications.Services.Dtos.Commons {
    /// <summary>
    /// 字典数据传输对象
    /// </summary>
    [DataContract]
    public class DicDto : DtoBase, ITreeNode {
        /// <summary>
        /// 父编号
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }

        /// <summary>
        /// 租户编号
        /// </summary>
        [DataMember]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength( 10, ErrorMessage = "编码输入过长，不能超过10位" )]
        [Display( Name = "编码" )]
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [Required( ErrorMessage = "文本不能为空" )]
        [StringLength( 50, ErrorMessage = "文本输入过长，不能超过50位" )]
        [Display( Name = "文本" )]
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [Required( ErrorMessage = "路径不能为空" )]
        [StringLength( 800, ErrorMessage = "路径输入过长，不能超过800位" )]
        [Display( Name = "路径" )]
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 级数
        /// </summary>
        [Required( ErrorMessage = "级数不能为空" )]
        [Display( Name = "级数" )]
        [DataMember]
        public int? Level { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Required( ErrorMessage = "排序号不能为空" )]
        [Display( Name = "排序号" )]
        [DataMember]
        public int? SortId { get; set; }

        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required( ErrorMessage = "拼音简码不能为空" )]
        [StringLength( 50, ErrorMessage = "拼音简码输入过长，不能超过50位" )]
        [Display( Name = "拼音简码" )]
        [DataMember]
        public string PinYin { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Required( ErrorMessage = "启用不能为空" )]
        [Display( Name = "启用" )]
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required( ErrorMessage = "创建时间不能为空" )]
        [Display( Name = "创建时间" )]
        [DataMember]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember]
        public Byte[] Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        [Json( PropertyName = "state" )]
        public string State { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        [DataMember]
        [Json( PropertyName = "children" )]
        public List<ITreeNode> Children { get; set; }

        /// <summary>
        /// 输出字典状态
        /// </summary>
        public override string ToString() {
            return this.ToEntity().ToString();
        }
    }
}
