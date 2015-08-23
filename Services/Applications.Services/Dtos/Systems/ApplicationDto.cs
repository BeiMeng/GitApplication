using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Util.ApplicationServices;
using Util;
namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 应用程序数据传输对象
    /// </summary>
    [DataContract]
    public class ApplicationDto : DtoBase {
        /// <summary>
        /// 应用程序编码
        /// </summary>
        [Required(ErrorMessage = "应用程序编码不能为空")]
        [StringLength( 10, ErrorMessage = "应用程序编码输入过长，不能超过10位" )]
        [Display( Name = "应用程序编码" )]
        [DataMember]
        public string Code { get; set; }
        
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [Required(ErrorMessage = "应用程序名称不能为空")]
        [StringLength( 30, ErrorMessage = "应用程序名称输入过长，不能超过30位" )]
        [Display( Name = "应用程序名称" )]
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        [Display( Name = "备注" )]
        [DataMember]
        public string Note { get; set; }
        
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
        /// 是否选中，UI上使用
        /// </summary>
        [Display(Name = "是否选中")]
        [DataMember]
        public bool Checked { get; set; }
        /// <summary>
        /// 租户编号(当前需设置应用程序的租户编号)
        /// </summary>
        [DataMember]
        public Guid TenantId { get; set; }
        /// <summary>
        /// 输出应用程序状态
        /// </summary>
        public override string ToString() {
            return this.ToEntity().ToString();
        }
    }
}
