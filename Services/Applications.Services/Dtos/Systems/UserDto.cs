using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Util.ApplicationServices;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 用户数据传输对象
    /// </summary>
    [DataContract]
    public class UserDto : DtoBase {
        [DataMember]
        public Guid? TenantId { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength( 100, ErrorMessage = "用户名输入过长，不能超过100位" )]
        [Display( Name = "用户名" )]
        [DataMember]
        public string UserName { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength( 40, ErrorMessage = "密码输入过长，不能超过40位" )]
        [Display( Name = "密码" )]
        [DataMember]
        public string Password { get; set; }
        
        /// <summary>
        /// 安全码
        /// </summary>
        [StringLength( 40, ErrorMessage = "安全码输入过长，不能超过40位" )]
        [Display( Name = "安全码" )]
        [DataMember]
        public string SafePassword { get; set; }
        
        /// <summary>
        /// 安全邮箱
        /// </summary>
        [StringLength( 200, ErrorMessage = "安全邮箱输入过长，不能超过200位" )]
        [Display( Name = "安全邮箱" )]
        [DataMember]
        public string Email { get; set; }
        
        /// <summary>
        /// 安全手机
        /// </summary>
        [StringLength( 20, ErrorMessage = "安全手机输入过长，不能超过20位" )]
        [Display( Name = "安全手机" )]
        [DataMember]
        public string MobilePhone { get; set; }
        
        /// <summary>
        /// 密码问题
        /// </summary>
        [StringLength( 100, ErrorMessage = "密码问题输入过长，不能超过100位" )]
        [Display( Name = "密码问题" )]
        [DataMember]
        public string Question { get; set; }
        
        /// <summary>
        /// 密码答案
        /// </summary>
        [StringLength( 100, ErrorMessage = "密码答案输入过长，不能超过100位" )]
        [Display( Name = "密码答案" )]
        [DataMember]
        public string Answer { get; set; }
        
        /// <summary>
        /// 锁定
        /// </summary>
        [Required(ErrorMessage = "锁定不能为空")]
        [Display( Name = "锁定" )]
        [DataMember]
        public bool IsLock { get; set; }
        
        /// <summary>
        /// 锁定起始时间
        /// </summary>
        [Display( Name = "锁定起始时间" )]
        [DataMember]
        public DateTime? LockBeginTime { get; set; }
        
        /// <summary>
        /// 锁定持续时间
        /// </summary>
        [Display( Name = "锁定持续时间" )]
        [DataMember]
        public int? LockTime { get; set; }
        
        /// <summary>
        /// 锁定提示消息
        /// </summary>
        [StringLength( 100, ErrorMessage = "锁定提示消息输入过长，不能超过100位" )]
        [Display( Name = "锁定提示消息" )]
        [DataMember]
        public string LockMessage { get; set; }
        
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        [Display( Name = "上次登陆时间" )]
        [DataMember]
        public DateTime? LastLoginTime { get; set; }
        
        /// <summary>
        /// 上次登陆Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "上次登陆Ip输入过长，不能超过30位" )]
        [Display( Name = "上次登陆Ip" )]
        [DataMember]
        public string LastLoginIp { get; set; }
        
        /// <summary>
        /// 本次登陆时间
        /// </summary>
        [Display( Name = "本次登陆时间" )]
        [DataMember]
        public DateTime? CurrentLoginTime { get; set; }
        
        /// <summary>
        /// 本次登陆Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "本次登陆Ip输入过长，不能超过30位" )]
        [Display( Name = "本次登陆Ip" )]
        [DataMember]
        public string CurrentLoginIp { get; set; }
        
        /// <summary>
        /// 登陆次数
        /// </summary>
        [Display( Name = "登陆次数" )]
        [DataMember]
        public int? LoginTimes { get; set; }
        
        /// <summary>
        /// 登陆失败次数
        /// </summary>
        [Display( Name = "登陆失败次数" )]
        [DataMember]
        public int? LoginFailTimes { get; set; }
        
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
        /// 冻结时间
        /// </summary>
        [Display( Name = "冻结时间" )]
        [DataMember]
        public DateTime? DisableTime { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        [Display( Name = "创建时间" )]
        [DataMember]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 注册Ip
        /// </summary>
        [StringLength( 30, ErrorMessage = "注册Ip输入过长，不能超过30位" )]
        [Display( Name = "注册Ip" )]
        [DataMember]
        public string RegisterIp { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        [Display( Name = "版本号" )]
        [DataMember]
        public Byte[] Version { get; set; }
        
        /// <summary>
        /// 输出用户状态
        /// </summary>
        public override string ToString() {
            return this.ToEntity().ToString();
        }
    }
}
