using System;
using System.ComponentModel.DataAnnotations;
using Util;
using Util.Domains.Repositories;

namespace Applications.Domains.Queries.Systems {
    /// <summary>
    /// 角色查询实体
    /// </summary>
    public class RoleQuery : Pager {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Display(Name="角色编号")]
        public Guid? RoleId { get; set; }
        
        /// <summary>
        /// 租户编号
        /// </summary>
        [Display(Name="租户编号")]
        public Guid? TenantId { get; set; }
        
        private string _code = string.Empty;
        /// <summary>
        /// 角色编码
        /// </summary>
        [Display(Name="角色编码")]
        public string Code {
            get { return _code == null ? string.Empty : _code.Trim(); }
            set{ _code=value;}
        }
        
        private string _name = string.Empty;
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name="角色名称")]
        public string Name {
            get { return _name == null ? string.Empty : _name.Trim(); }
            set{ _name=value;}
        }
        
        /// <summary>
        /// 角色类型
        /// </summary>
        [Display(Name="角色类型")]
        public Guid? Type { get; set; }
        
        private string _typeName = string.Empty;
        /// <summary>
        /// 角色类型名称
        /// </summary>
        [Display(Name="角色类型名称")]
        public string TypeName {
            get { return _typeName == null ? string.Empty : _typeName.Trim(); }
            set{ _typeName=value;}
        }
        
        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name="管理员")]
        public bool? IsAdmin { get; set; }
        
        private string _note = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name="备注")]
        public string Note {
            get { return _note == null ? string.Empty : _note.Trim(); }
            set{ _note=value;}
        }
        
        private string _pinYin = string.Empty;
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Display(Name="拼音简码")]
        public string PinYin {
            get { return _pinYin == null ? string.Empty : _pinYin.Trim(); }
            set{ _pinYin=value;}
        }
        
        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name="启用")]
        public bool? Enabled { get; set; }
        
        /// <summary>
        /// 起始创建时间
        /// </summary>
        [Display( Name = "起始创建时间" )]
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 结束创建时间
        /// </summary>
        [Display( Name = "结束创建时间" )]
        public DateTime? EndCreateTime { get; set; }
        private string _sign = string.Empty;
        /// <summary>
        /// 签名
        /// </summary>
        [Display(Name="签名")]
        public string Sign {
            get { return _sign == null ? string.Empty : _sign.Trim(); }
            set{ _sign=value;}
        }
        
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            base.AddDescriptions();
            AddDescription( "角色编号", RoleId ); 
            AddDescription( "租户编号", TenantId ); 
            AddDescription( "角色编码", Code ); 
            AddDescription( "角色名称", Name ); 
            AddDescription( "角色类型", Type ); 
            AddDescription( "角色类型名称", TypeName ); 
            AddDescription( "管理员", IsAdmin.Description() ); 
            AddDescription( "备注", Note ); 
            AddDescription( "拼音简码", PinYin ); 
            AddDescription( "启用", Enabled.Description() ); 
            AddDescription( "起始创建时间", BeginCreateTime );
            AddDescription( "结束创建时间", EndCreateTime );
            AddDescription( "签名", Sign ); 
        } 
    }
}
