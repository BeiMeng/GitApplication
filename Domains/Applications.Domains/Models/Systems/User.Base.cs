using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Applications.Domains.Core.Tenant.Model;
using Util;
using Util.Domains;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 用户
    /// </summary>
    [DisplayName( "用户" )]
    public partial class User : TenantAggregateRoot<User>
    {
        /// <summary>
        /// 初始化用户
        /// </summary>
        public User() : this( Guid.Empty ) {
        }

        /// <summary>
        /// 初始化用户
        /// </summary>
        /// <param name="id">用户标识</param>
        public User( Guid id ) : base( id ) {
            Roles = new List<Role>();
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength( 100, ErrorMessage = "用户名输入过长，不能超过100位" )]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength( 40, ErrorMessage = "密码输入过长，不能超过40位" )]
        public string Password { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        [StringLength( 40, ErrorMessage = "安全码输入过长，不能超过40位" )]
        public string SafePassword { get; set; }
        /// <summary>
        /// 安全邮箱
        /// </summary>
        [StringLength( 200, ErrorMessage = "安全邮箱输入过长，不能超过200位" )]
        public string Email { get; set; }
        /// <summary>
        /// 安全手机
        /// </summary>
        [StringLength( 20, ErrorMessage = "安全手机输入过长，不能超过20位" )]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 密码问题
        /// </summary>
        [StringLength( 100, ErrorMessage = "密码问题输入过长，不能超过100位" )]
        public string Question { get; set; }
        /// <summary>
        /// 密码答案
        /// </summary>
        [StringLength( 100, ErrorMessage = "密码答案输入过长，不能超过100位" )]
        public string Answer { get; set; }
        /// <summary>
        /// 锁定
        /// </summary>
        [Required(ErrorMessage = "锁定不能为空")]
        public bool IsLock { get; set; }
        /// <summary>
        /// 锁定起始时间
        /// </summary>
        public DateTime? LockBeginTime { get; set; }
        /// <summary>
        /// 锁定持续时间
        /// </summary>
        public int? LockTime { get; set; }
        /// <summary>
        /// 锁定提示消息
        /// </summary>
        [StringLength( 100, ErrorMessage = "锁定提示消息输入过长，不能超过100位" )]
        public string LockMessage { get; set; }
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 上次登陆Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "上次登陆Ip输入过长，不能超过30位" )]
        public string LastLoginIp { get; set; }
        /// <summary>
        /// 本次登陆时间
        /// </summary>
        public DateTime? CurrentLoginTime { get; set; }
        /// <summary>
        /// 本次登陆Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "本次登陆Ip输入过长，不能超过30位" )]
        public string CurrentLoginIp { get; set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public int? LoginTimes { get; set; }
        /// <summary>
        /// 登陆失败次数
        /// </summary>
        public int? LoginFailTimes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength( 100, ErrorMessage = "备注输入过长，不能超过100位" )]
        public string Note { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "启用不能为空")]
        public bool Enabled { get; set; }
        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime? DisableTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 注册Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "注册Ip输入过长，不能超过30位" )]
        public string RegisterIp { get; set; }


        /// <summary>
        /// 角色列表
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            AddDescription( "用户编号", Id );
            AddDescription( "租户编号", TenantId );
            AddDescription( "用户名", UserName );
            AddDescription( "密码", Password );
            AddDescription( "安全码", SafePassword );
            AddDescription( "安全邮箱", Email );
            AddDescription( "安全手机", MobilePhone );
            AddDescription( "密码问题", Question );
            AddDescription( "密码答案", Answer );
            AddDescription( "锁定", IsLock.Description() );
            AddDescription( "锁定起始时间", LockBeginTime );
            AddDescription( "锁定持续时间", LockTime );
            AddDescription( "锁定提示消息", LockMessage );
            AddDescription( "上次登陆时间", LastLoginTime );
            AddDescription( "上次登陆Ip", LastLoginIp );
            AddDescription( "本次登陆时间", CurrentLoginTime );
            AddDescription( "本次登陆Ip", CurrentLoginIp );
            AddDescription( "登陆次数", LoginTimes );
            AddDescription( "登陆失败次数", LoginFailTimes );
            AddDescription( "备注", Note );
            AddDescription( "启用", Enabled.Description() );
            AddDescription( "冻结时间", DisableTime );
            AddDescription( "创建时间", CreateTime );
            AddDescription( "注册Ip", RegisterIp );
        }
        
        /// <summary>
        /// 添加变更列表
        /// </summary>
        protected override void AddChanges( User newEntity ) {
            AddChange( "Id", "用户编号", Id, newEntity.Id );            
            AddChange( "TenantId", "租户编号", TenantId, newEntity.TenantId );
            AddChange( "UserName", "用户名", UserName, newEntity.UserName );
            AddChange( "Password", "密码", Password, newEntity.Password );
            AddChange( "SafePassword", "安全码", SafePassword, newEntity.SafePassword );
            AddChange( "Email", "安全邮箱", Email, newEntity.Email );
            AddChange( "MobilePhone", "安全手机", MobilePhone, newEntity.MobilePhone );
            AddChange( "Question", "密码问题", Question, newEntity.Question );
            AddChange( "Answer", "密码答案", Answer, newEntity.Answer );
            AddChange( "IsLock", "锁定", IsLock, newEntity.IsLock );
            AddChange( "LockBeginTime", "锁定起始时间", LockBeginTime, newEntity.LockBeginTime );
            AddChange( "LockTime", "锁定持续时间", LockTime, newEntity.LockTime );
            AddChange( "LockMessage", "锁定提示消息", LockMessage, newEntity.LockMessage );
            AddChange( "LastLoginTime", "上次登陆时间", LastLoginTime, newEntity.LastLoginTime );
            AddChange( "LastLoginIp", "上次登陆Ip", LastLoginIp, newEntity.LastLoginIp );
            AddChange( "CurrentLoginTime", "本次登陆时间", CurrentLoginTime, newEntity.CurrentLoginTime );
            AddChange( "CurrentLoginIp", "本次登陆Ip", CurrentLoginIp, newEntity.CurrentLoginIp );
            AddChange( "LoginTimes", "登陆次数", LoginTimes, newEntity.LoginTimes );
            AddChange( "LoginFailTimes", "登陆失败次数", LoginFailTimes, newEntity.LoginFailTimes );
            AddChange( "Note", "备注", Note, newEntity.Note );
            AddChange( "Enabled", "启用", Enabled, newEntity.Enabled );
            AddChange( "DisableTime", "冻结时间", DisableTime, newEntity.DisableTime );
            AddChange( "CreateTime", "创建时间", CreateTime, newEntity.CreateTime );
            AddChange( "RegisterIp", "注册Ip", RegisterIp, newEntity.RegisterIp );
            AddChange( "Version", "版本号", Version, newEntity.Version );
        }
    }
}