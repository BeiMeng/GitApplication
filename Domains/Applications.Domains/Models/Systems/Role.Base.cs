using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Applications.Domains.Core.Tenant.Model;
using Util;
using Util.Domains;
using Util.Security;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 角色
    /// </summary>
    [DisplayName( "角色" )]
    public partial class Role : TenantAggregateRoot<Role>
    {
        /// <summary>
        /// 初始化角色
        /// </summary>
        public Role() : this( Guid.Empty ) {
        }

        /// <summary>
        /// 初始化角色
        /// </summary>
        /// <param name="id">角色标识</param>
        public Role( Guid id ) : base( id ) {
            //Permissions = new List<Permission>();
            //Applications = new List<Application>();
            Users = new List<User>();
        }
        /// <summary>
        /// 角色编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空")]
        [StringLength( 10, ErrorMessage = "角色编码输入过长，不能超过10位" )]
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        [StringLength( 200, ErrorMessage = "角色名称输入过长，不能超过200位" )]
        public string Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        [Required(ErrorMessage = "角色类型不能为空")]
        public Guid Type { get; set; }
        /// <summary>
        /// 角色类型名称
        /// </summary>
        [StringLength( 50, ErrorMessage = "角色类型名称输入过长，不能超过50位" )]
        public string TypeName { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        [Required(ErrorMessage = "管理员不能为空")]
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        public string Note { get; set; }
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required(ErrorMessage = "拼音简码不能为空")]
        [StringLength( 30, ErrorMessage = "拼音简码输入过长，不能超过30位" )]
        public string PinYin { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "启用不能为空")]
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage = "签名不能为空")]
        [StringLength( 50, ErrorMessage = "签名输入过长，不能超过50位" )]
        public string Sign { get; set; }
        
        /// <summary>
        /// 权限列表
        /// </summary>
        //public virtual ICollection<Permission> Permissions { get; set; }
      
        
        /// <summary>
        /// 应用程序列表
        /// </summary>
        //public virtual ICollection<Application> Applications { get; set; }
        
        /// <summary>
        /// 用户列表
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            AddDescription( "角色编号", Id );
            AddDescription( "租户编号", TenantId );
            AddDescription( "角色编码", Code );
            AddDescription( "角色名称", Name );
            AddDescription( "角色类型", Type );
            AddDescription( "角色类型名称", TypeName );
            AddDescription( "管理员", IsAdmin.Description() );
            AddDescription( "备注", Note );
            AddDescription( "拼音简码", PinYin );
            AddDescription( "启用", Enabled.Description() );
            AddDescription( "创建时间", CreateTime );
            AddDescription( "签名", Sign );
        }
        
        /// <summary>
        /// 添加变更列表
        /// </summary>
        protected override void AddChanges( Role newEntity ) {
            AddChange( "Id", "角色编号", Id, newEntity.Id );            
            AddChange( "TenantId", "租户编号", TenantId, newEntity.TenantId );
            AddChange( "Code", "角色编码", Code, newEntity.Code );
            AddChange( "Name", "角色名称", Name, newEntity.Name );
            AddChange( "Type", "角色类型", Type, newEntity.Type );
            AddChange( "TypeName", "角色类型名称", TypeName, newEntity.TypeName );
            AddChange( "IsAdmin", "管理员", IsAdmin, newEntity.IsAdmin );
            AddChange( "Note", "备注", Note, newEntity.Note );
            AddChange( "PinYin", "拼音简码", PinYin, newEntity.PinYin );
            AddChange( "Enabled", "启用", Enabled, newEntity.Enabled );
            AddChange( "CreateTime", "创建时间", CreateTime, newEntity.CreateTime );
            AddChange( "Version", "版本号", Version, newEntity.Version );
            AddChange( "Sign", "签名", Sign, newEntity.Sign );
        }
    }
}