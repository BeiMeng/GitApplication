using Applications.Domains.Models.Systems;
using Util.Datas.Ef;

namespace Datas.Ef.Mappings.Systems {
    /// <summary>
    /// 用户映射
    /// </summary>
    public class UserMap : AggregateMapBase<User> {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable() {
            ToTable( "Users", "Systems" );
        }
        
        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties() {
            //用户编号
            Property(t => t.Id)
                .HasColumnName("UserId");
            //租户编号
            Property(t => t.TenantId)
                .HasColumnName("TenantId");
            //用户名
            Property(t => t.UserName)
                .HasColumnName("UserName");
            //密码
            Property(t => t.Password)
                .HasColumnName("Password");
            //安全码
            Property(t => t.SafePassword)
                .HasColumnName("SafePassword");
            //安全邮箱
            Property(t => t.Email)
                .HasColumnName("Email");
            //安全手机
            Property(t => t.MobilePhone)
                .HasColumnName("MobilePhone");
            //密码问题
            Property(t => t.Question)
                .HasColumnName("Question");
            //密码答案
            Property(t => t.Answer)
                .HasColumnName("Answer");
            //锁定
            Property(t => t.IsLock)
                .HasColumnName("IsLock");
            //锁定起始时间
            Property(t => t.LockBeginTime)
                .HasColumnName("LockBeginTime");
            //锁定持续时间
            Property(t => t.LockTime)
                .HasColumnName("LockTime");
            //锁定提示消息
            Property(t => t.LockMessage)
                .HasColumnName("LockMessage");
            //上次登陆时间
            Property(t => t.LastLoginTime)
                .HasColumnName("LastLoginTime");
            //上次登陆Ip
            Property(t => t.LastLoginIp)
                .HasColumnName("LastLoginIp");
            //本次登陆时间
            Property(t => t.CurrentLoginTime)
                .HasColumnName("CurrentLoginTime");
            //本次登陆Ip
            Property(t => t.CurrentLoginIp)
                .HasColumnName("CurrentLoginIp");
            //登陆次数
            Property(t => t.LoginTimes)
                .HasColumnName("LoginTimes");
            //登陆失败次数
            Property(t => t.LoginFailTimes)
                .HasColumnName("LoginFailTimes");
            //备注
            Property(t => t.Note)
                .HasColumnName("Note");
            //启用
            Property(t => t.Enabled)
                .HasColumnName("Enabled");
            //冻结时间
            Property(t => t.DisableTime)
                .HasColumnName("DisableTime");
            //创建时间
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime");
            //注册Ip
            Property(t => t.RegisterIp)
                .HasColumnName("RegisterIp");
        }
        
        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected override void MapAssociations() {
            //租户
            HasRequired(t => t.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.TenantId);
        }
    }
}
