using System;
using System.ComponentModel.DataAnnotations;
using Util;
using Util.Domains.Repositories;

namespace Applications.Domains.Queries.Systems {
    /// <summary>
    /// 用户查询实体
    /// </summary>
    public class UserQuery : Pager {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name="用户编号")]
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// 租户编号
        /// </summary>
        [Display(Name="租户编号")]
        public Guid? TenantId { get; set; }
        
        private string _userName = string.Empty;
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name="用户名")]
        public string UserName {
            get { return _userName == null ? string.Empty : _userName.Trim(); }
            set{ _userName=value;}
        }
        
        private string _password = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name="密码")]
        public string Password {
            get { return _password == null ? string.Empty : _password.Trim(); }
            set{ _password=value;}
        }
        
        private string _safePassword = string.Empty;
        /// <summary>
        /// 安全码
        /// </summary>
        [Display(Name="安全码")]
        public string SafePassword {
            get { return _safePassword == null ? string.Empty : _safePassword.Trim(); }
            set{ _safePassword=value;}
        }
        
        private string _email = string.Empty;
        /// <summary>
        /// 安全邮箱
        /// </summary>
        [Display(Name="安全邮箱")]
        public string Email {
            get { return _email == null ? string.Empty : _email.Trim(); }
            set{ _email=value;}
        }
        
        private string _mobilePhone = string.Empty;
        /// <summary>
        /// 安全手机
        /// </summary>
        [Display(Name="安全手机")]
        public string MobilePhone {
            get { return _mobilePhone == null ? string.Empty : _mobilePhone.Trim(); }
            set{ _mobilePhone=value;}
        }
        
        private string _question = string.Empty;
        /// <summary>
        /// 密码问题
        /// </summary>
        [Display(Name="密码问题")]
        public string Question {
            get { return _question == null ? string.Empty : _question.Trim(); }
            set{ _question=value;}
        }
        
        private string _answer = string.Empty;
        /// <summary>
        /// 密码答案
        /// </summary>
        [Display(Name="密码答案")]
        public string Answer {
            get { return _answer == null ? string.Empty : _answer.Trim(); }
            set{ _answer=value;}
        }
        
        /// <summary>
        /// 锁定
        /// </summary>
        [Display(Name="锁定")]
        public bool? IsLock { get; set; }
        
        /// <summary>
        /// 起始锁定起始时间
        /// </summary>
        [Display( Name = "起始锁定起始时间" )]
        public DateTime? BeginLockBeginTime { get; set; }

        /// <summary>
        /// 结束锁定起始时间
        /// </summary>
        [Display( Name = "结束锁定起始时间" )]
        public DateTime? EndLockBeginTime { get; set; }
        /// <summary>
        /// 锁定持续时间
        /// </summary>
        [Display(Name="锁定持续时间")]
        public int? LockTime { get; set; }
        
        private string _lockMessage = string.Empty;
        /// <summary>
        /// 锁定提示消息
        /// </summary>
        [Display(Name="锁定提示消息")]
        public string LockMessage {
            get { return _lockMessage == null ? string.Empty : _lockMessage.Trim(); }
            set{ _lockMessage=value;}
        }
        
        /// <summary>
        /// 起始上次登陆时间
        /// </summary>
        [Display( Name = "起始上次登陆时间" )]
        public DateTime? BeginLastLoginTime { get; set; }

        /// <summary>
        /// 结束上次登陆时间
        /// </summary>
        [Display( Name = "结束上次登陆时间" )]
        public DateTime? EndLastLoginTime { get; set; }
        private string _lastLoginIp = string.Empty;
        /// <summary>
        /// 上次登陆Ip
        /// </summary>
        [Display(Name="上次登陆Ip")]
        public string LastLoginIp {
            get { return _lastLoginIp == null ? string.Empty : _lastLoginIp.Trim(); }
            set{ _lastLoginIp=value;}
        }
        
        /// <summary>
        /// 起始本次登陆时间
        /// </summary>
        [Display( Name = "起始本次登陆时间" )]
        public DateTime? BeginCurrentLoginTime { get; set; }

        /// <summary>
        /// 结束本次登陆时间
        /// </summary>
        [Display( Name = "结束本次登陆时间" )]
        public DateTime? EndCurrentLoginTime { get; set; }
        private string _currentLoginIp = string.Empty;
        /// <summary>
        /// 本次登陆Ip
        /// </summary>
        [Display(Name="本次登陆Ip")]
        public string CurrentLoginIp {
            get { return _currentLoginIp == null ? string.Empty : _currentLoginIp.Trim(); }
            set{ _currentLoginIp=value;}
        }
        
        /// <summary>
        /// 登陆次数
        /// </summary>
        [Display(Name="登陆次数")]
        public int? LoginTimes { get; set; }
        
        /// <summary>
        /// 登陆失败次数
        /// </summary>
        [Display(Name="登陆失败次数")]
        public int? LoginFailTimes { get; set; }
        
        private string _note = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name="备注")]
        public string Note {
            get { return _note == null ? string.Empty : _note.Trim(); }
            set{ _note=value;}
        }
        
        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name="启用")]
        public bool? Enabled { get; set; }
        
        /// <summary>
        /// 起始冻结时间
        /// </summary>
        [Display( Name = "起始冻结时间" )]
        public DateTime? BeginDisableTime { get; set; }

        /// <summary>
        /// 结束冻结时间
        /// </summary>
        [Display( Name = "结束冻结时间" )]
        public DateTime? EndDisableTime { get; set; }
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
        private string _registerIp = string.Empty;
        /// <summary>
        /// 注册Ip
        /// </summary>
        [Display(Name="注册Ip")]
        public string RegisterIp {
            get { return _registerIp == null ? string.Empty : _registerIp.Trim(); }
            set{ _registerIp=value;}
        }
        
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            base.AddDescriptions();
            AddDescription( "用户编号", UserId ); 
            AddDescription( "租户编号", TenantId ); 
            AddDescription( "用户名", UserName ); 
            AddDescription( "密码", Password ); 
            AddDescription( "安全码", SafePassword ); 
            AddDescription( "安全邮箱", Email ); 
            AddDescription( "安全手机", MobilePhone ); 
            AddDescription( "密码问题", Question ); 
            AddDescription( "密码答案", Answer ); 
            AddDescription( "锁定", IsLock.Description() ); 
            AddDescription( "起始锁定起始时间", BeginLockBeginTime );
            AddDescription( "结束锁定起始时间", EndLockBeginTime );
            AddDescription( "锁定持续时间", LockTime ); 
            AddDescription( "锁定提示消息", LockMessage ); 
            AddDescription( "起始上次登陆时间", BeginLastLoginTime );
            AddDescription( "结束上次登陆时间", EndLastLoginTime );
            AddDescription( "上次登陆Ip", LastLoginIp ); 
            AddDescription( "起始本次登陆时间", BeginCurrentLoginTime );
            AddDescription( "结束本次登陆时间", EndCurrentLoginTime );
            AddDescription( "本次登陆Ip", CurrentLoginIp ); 
            AddDescription( "登陆次数", LoginTimes ); 
            AddDescription( "登陆失败次数", LoginFailTimes ); 
            AddDescription( "备注", Note ); 
            AddDescription( "启用", Enabled.Description() ); 
            AddDescription( "起始冻结时间", BeginDisableTime );
            AddDescription( "结束冻结时间", EndDisableTime );
            AddDescription( "起始创建时间", BeginCreateTime );
            AddDescription( "结束创建时间", EndCreateTime );
            AddDescription( "注册Ip", RegisterIp ); 
        } 
    }
}
