using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Applications.Domains.Enums;
using Applications.Domains.Models.Commons;
using Util;
using Util.Domains;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 租户
    /// </summary>
    [DisplayName( "租户" )]
    public partial class Tenant : TreeEntityBase<Tenant>
    {
        /// <summary>
        /// 初始化租户
        /// </summary>
        public Tenant()
            : this(Guid.Empty, "", 0)
        {
        }

        /// <summary>
        /// 初始化租户
        /// </summary>
        /// <param name="id">租户标识</param>
        public Tenant(Guid id, string path, int level)
            : base( id, path,level ) {
                Roles = new List<Role>();
                //SiteInfos = new List<SiteInfo>();
                Applications = new List<Application>();
                Users = new List<User>();
                Dics=new List<Dic>();
        }
        /// <summary>
        /// 租户编码
        /// </summary>
        [Required(ErrorMessage = "租户编码不能为空")]
        [StringLength( 6, ErrorMessage = "租户编码输入过长，不能超过6位" )]
        public string Code { get; set; }
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required(ErrorMessage = "租户名称不能为空")]
        [StringLength( 100, ErrorMessage = "租户名称输入过长，不能超过100位" )]
        public string Name { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Required(ErrorMessage = "联系人姓名不能为空")]
        [StringLength( 30, ErrorMessage = "联系人姓名输入过长，不能超过30位" )]
        public string ContactName { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不能为空")]
        [StringLength( 200, ErrorMessage = "电子邮件输入过长，不能超过200位" )]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength( 30, ErrorMessage = "电话输入过长，不能超过30位" )]
        public string Phone { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "手机不能为空")]
        [StringLength( 30, ErrorMessage = "手机输入过长，不能超过30位" )]
        [MobilePhone]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        [StringLength( 30, ErrorMessage = "传真输入过长，不能超过30位" )]
        public string Fax { get; set; }
        /// <summary>
        /// Qq
        /// </summary>
        [StringLength( 15, ErrorMessage = "Qq输入过长，不能超过15位" )]
        public string Qq { get; set; }
        /// <summary>
        /// 省份编号
        /// </summary>
        public Guid? ProvinceId { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [StringLength( 100, ErrorMessage = "省份输入过长，不能超过100位" )]
        public string Province { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public Guid? CityId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [StringLength( 100, ErrorMessage = "城市输入过长，不能超过100位" )]
        public string City { get; set; }
        /// <summary>
        /// 区县编号
        /// </summary>
        public Guid? CountyId { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        [StringLength( 100, ErrorMessage = "区县输入过长，不能超过100位" )]
        public string County { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        [StringLength( 200, ErrorMessage = "街道输入过长，不能超过200位" )]
        public string Street { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        [StringLength( 20, ErrorMessage = "邮政编码输入过长，不能超过20位" )]
        public string Zip { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public TenantState TState { get; set; }

        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required(ErrorMessage = "拼音简码不能为空")]
        [StringLength( 100, ErrorMessage = "拼音简码输入过长，不能超过100位" )]
        public string PinYin { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        public string Note { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
        
        ///// <summary>
        ///// 站点信息列表
        ///// </summary>
        //public virtual ICollection<SiteInfo> SiteInfos { get; set; }

        /// <summary>
        /// 应用程序列表
        /// </summary>
        public virtual ICollection<Application> Applications { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        /// <summary>
        /// 字典列表
        /// </summary>
        public virtual ICollection<Dic> Dics { get; set; }
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            AddDescription( "租户编号", Id );
            AddDescription( "租户编码", Code );
            AddDescription( "租户名称", Name );
            AddDescription( "父编号", ParentId );
            AddDescription( "路径", Path );
            AddDescription( "级数", Level );
            AddDescription( "联系人姓名", ContactName );
            AddDescription( "电子邮件", Email );
            AddDescription( "电话", Phone );
            AddDescription( "手机", MobilePhone );
            AddDescription( "传真", Fax );
            AddDescription( "Qq", Qq );
            AddDescription( "省份编号", ProvinceId );
            AddDescription( "省份", Province );
            AddDescription( "城市编号", CityId );
            AddDescription( "城市", City );
            AddDescription( "区县编号", CountyId );
            AddDescription( "区县", County );
            AddDescription( "街道", Street );
            AddDescription( "邮政编码", Zip );
            AddDescription( "启用", Enabled.Description() );
            AddDescription( "状态", TState );
            AddDescription( "排序号", SortId );
            AddDescription( "拼音简码", PinYin );
            AddDescription( "备注", Note );
            AddDescription( "创建时间", CreateTime );
        }
        
        /// <summary>
        /// 添加变更列表
        /// </summary>
        protected override void AddChanges( Tenant newEntity ) {
            AddChange( "Id", "租户编号", Id, newEntity.Id );            
            AddChange( "Code", "租户编码", Code, newEntity.Code );
            AddChange( "Name", "租户名称", Name, newEntity.Name );
            AddChange( "ParentId", "父编号", ParentId, newEntity.ParentId );
            AddChange( "Path", "路径", Path, newEntity.Path );
            AddChange( "Level", "级数", Level, newEntity.Level );
            AddChange( "ContactName", "联系人姓名", ContactName, newEntity.ContactName );
            AddChange( "Email", "电子邮件", Email, newEntity.Email );
            AddChange( "Phone", "电话", Phone, newEntity.Phone );
            AddChange( "MobilePhone", "手机", MobilePhone, newEntity.MobilePhone );
            AddChange( "Fax", "传真", Fax, newEntity.Fax );
            AddChange( "Qq", "Qq", Qq, newEntity.Qq );
            AddChange( "ProvinceId", "省份编号", ProvinceId, newEntity.ProvinceId );
            AddChange( "Province", "省份", Province, newEntity.Province );
            AddChange( "CityId", "城市编号", CityId, newEntity.CityId );
            AddChange( "City", "城市", City, newEntity.City );
            AddChange( "CountyId", "区县编号", CountyId, newEntity.CountyId );
            AddChange( "County", "区县", County, newEntity.County );
            AddChange( "Street", "街道", Street, newEntity.Street );
            AddChange( "Zip", "邮政编码", Zip, newEntity.Zip );
            AddChange( "Enabled", "启用", Enabled, newEntity.Enabled );
            AddChange( "State", "状态", TState, newEntity.TState );
            AddChange( "SortId", "排序号", SortId, newEntity.SortId );
            AddChange( "PinYin", "拼音简码", PinYin, newEntity.PinYin );
            AddChange( "Note", "备注", Note, newEntity.Note );
            AddChange( "CreateTime", "创建时间", CreateTime, newEntity.CreateTime );
            AddChange( "Version", "版本号", Version, newEntity.Version );
        }
    }
}