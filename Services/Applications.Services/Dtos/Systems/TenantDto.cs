using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Applications.Domains.Enums;
using Util;
using Util.ApplicationServices;
using Util.Webs.Controls;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 租户数据传输对象
    /// </summary>
    [DataContract]
    public class TenantDto : DtoBase, ITreeNode
    {
        /// <summary>
        /// 租户编码
        /// </summary>
        [Required(ErrorMessage = "租户编码不能为空")]
        [StringLength( 6, ErrorMessage = "租户编码输入过长，不能超过6位" )]
        [Display( Name = "租户编码" )]
        [DataMember]
        public string Code { get; set; }
        
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required(ErrorMessage = "租户名称不能为空")]
        [StringLength( 100, ErrorMessage = "租户名称输入过长，不能超过100位" )]
        [Display( Name = "租户名称" )]
        [DataMember]
        public string Text { get; set; }
        
        /// <summary>
        /// 父编号
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }
        
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }
        
        /// <summary>
        /// 排序号
        /// </summary>
        [Required(ErrorMessage = "排序号不能为空")]
        [Display(Name = "排序号")]
        [DataMember]
        public int? SortId { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength( 30, ErrorMessage = "联系人姓名输入过长，不能超过30位" )]
        [Required(ErrorMessage = "联系人姓名不能为空")]
        [Display( Name = "联系人姓名" )]
        [DataMember]
        public string ContactName { get; set; }
        
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不能为空")]
        [StringLength(200, ErrorMessage = "电子邮件输入过长，不能超过200位")]
        [EmailAddress]
        [Display( Name = "电子邮件" )]
        
        [DataMember]
        public string Email { get; set; }
        
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength( 30, ErrorMessage = "电话输入过长，不能超过30位" )]
        [Display( Name = "电话" )]
        [DataMember]
        public string Phone { get; set; }
        
        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "手机不能为空")]
        [StringLength(30, ErrorMessage = "手机输入过长，不能超过30位")]
        [MobilePhone]
        [Display( Name = "手机" )]
        [DataMember]
        public string MobilePhone { get; set; }
        
        /// <summary>
        /// 传真
        /// </summary>
        [StringLength( 30, ErrorMessage = "传真输入过长，不能超过30位" )]
        [Display( Name = "传真" )]
        [DataMember]
        public string Fax { get; set; }
        
        /// <summary>
        /// Qq
        /// </summary>
        [StringLength( 15, ErrorMessage = "Qq输入过长，不能超过15位" )]
        [Display( Name = "Qq" )]
        [DataMember]
        public string Qq { get; set; }
        
        /// <summary>
        /// 省份编号
        /// </summary>
        [Display( Name = "省份编号" )]
        [DataMember]
        public Guid? ProvinceId { get; set; }
        
        /// <summary>
        /// 省份
        /// </summary>
        [StringLength( 100, ErrorMessage = "省份输入过长，不能超过100位" )]
        [Display( Name = "省份" )]
        [DataMember]
        public string Province { get; set; }
        
        /// <summary>
        /// 城市编号
        /// </summary>
        [Display( Name = "城市编号" )]
        [DataMember]
        public Guid? CityId { get; set; }
        
        /// <summary>
        /// 城市
        /// </summary>
        [StringLength( 100, ErrorMessage = "城市输入过长，不能超过100位" )]
        [Display( Name = "城市" )]
        [DataMember]
        public string City { get; set; }
        
        /// <summary>
        /// 区县编号
        /// </summary>
        [Display( Name = "区县编号" )]
        [DataMember]
        public Guid? CountyId { get; set; }
        
        /// <summary>
        /// 区县
        /// </summary>
        [StringLength( 100, ErrorMessage = "区县输入过长，不能超过100位" )]
        [Display( Name = "区县" )]
        [DataMember]
        public string County { get; set; }
        
        /// <summary>
        /// 街道
        /// </summary>
        [StringLength( 200, ErrorMessage = "街道输入过长，不能超过200位" )]
        [Display( Name = "街道" )]
        [DataMember]
        public string Street { get; set; }
        
        /// <summary>
        /// 邮政编码
        /// </summary>
        [StringLength( 20, ErrorMessage = "邮政编码输入过长，不能超过20位" )]
        [Display( Name = "邮政编码" )]
        [DataMember]
        public string Zip { get; set; }
        
        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "启用不能为空")]
        [Display( Name = "启用" )]
        [DataMember]
        public bool Enabled { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        [Display( Name = "状态" )]
        [DataMember]
        public TenantState TState { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        [Json(PropertyName = "state")]
        public string State { get; set; }
     
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required(ErrorMessage = "拼音简码不能为空")]
        [StringLength( 100, ErrorMessage = "拼音简码输入过长，不能超过100位" )]
        [Display( Name = "拼音简码" )]
        [DataMember]
        public string PinYin { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        [Display( Name = "备注" )]
        [DataMember]
        public string Note { get; set; }
        
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
        [DataMember]
        public Byte[] Version { get; set; }
        
        /// <summary>
        /// 输出租户状态
        /// </summary>
        public override string ToString() {
            return this.ToEntity().ToString();
        }

        /// <summary>
        /// 子节点集合
        /// </summary>
        [DataMember]
        [Json(PropertyName = "children")]
        public List<ITreeNode> Children { get; set; }


        [DataMember]
        public int? Level { get; set; }
    }
}
