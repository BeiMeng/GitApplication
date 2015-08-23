using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Util.ApplicationServices;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 角色数据传输对象
    /// </summary>
    [DataContract]
    public class RoleDto : DtoBase {
        /// <summary>
        /// 租户编号
        /// </summary>
        [Required(ErrorMessage = "租户编号不能为空")]
        [Display( Name = "租户编号" )]
        [DataMember]
        public Guid? TenantId { get; set; }
        
        /// <summary>
        /// 角色编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空")]
        [StringLength( 10, ErrorMessage = "角色编码输入过长，不能超过10位" )]
        [Display( Name = "角色编码" )]
        [DataMember]
        public string Code { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        [StringLength( 200, ErrorMessage = "角色名称输入过长，不能超过200位" )]
        [Display( Name = "角色名称" )]
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// 角色类型
        /// </summary>
        [Required(ErrorMessage = "角色类型不能为空")]
        [Display( Name = "角色类型" )]
        [DataMember]
        public Guid Type { get; set; }
        
        /// <summary>
        /// 角色类型名称
        /// </summary>
        [StringLength( 50, ErrorMessage = "角色类型名称输入过长，不能超过50位" )]
        [Display( Name = "角色类型名称" )]
        [DataMember]
        public string TypeName { get; set; }
        
        /// <summary>
        /// 管理员
        /// </summary>
        [Required(ErrorMessage = "管理员不能为空")]
        [Display( Name = "管理员" )]
        [DataMember]
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        [Display( Name = "备注" )]
        [DataMember]
        public string Note { get; set; }
        
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required(ErrorMessage = "拼音简码不能为空")]
        [StringLength( 30, ErrorMessage = "拼音简码输入过长，不能超过30位" )]
        [Display( Name = "拼音简码" )]
        [DataMember]
        public string PinYin { get; set; }
        
        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "启用不能为空")]
        [Display( Name = "启用" )]
        [DataMember]
        public bool Enabled { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        [Display( Name = "创建时间" )]
        [DataMember]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        [Display( Name = "版本号" )]
        [DataMember]
        public Byte[] Version { get; set; }
        
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage = "签名不能为空")]
        [StringLength( 50, ErrorMessage = "签名输入过长，不能超过50位" )]
        [Display( Name = "签名" )]
        [DataMember]
        public string Sign { get; set; }

        /// <summary>
        /// 是否选中，UI上使用
        /// </summary>
        [DataMember]
        public bool Checked { get; set; }
        /// <summary>
        ///用户编号(当前需设置应用程序的用户编号)
        /// </summary>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        /// 输出角色状态
        /// </summary>
        public override string ToString() {
            return this.ToEntity().ToString();
        }
    }
}
