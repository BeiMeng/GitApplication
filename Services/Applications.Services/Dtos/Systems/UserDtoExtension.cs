using Applications.Domains.Models.Systems;
using Util;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 用户数据传输对象扩展
    /// </summary>
    public static class UserDtoExtension {
        /// <summary>
        /// 转换为用户实体
        /// </summary>
        /// <param name="dto">用户数据传输对象</param>
        public static User ToEntity( this UserDto dto ) {
            if( dto == null )
                return new User();
            return new User( dto.Id.ToGuid() ) {
                TenantId = dto.TenantId,
                UserName = dto.UserName,
                Password = dto.Password,
                SafePassword = dto.SafePassword,
                Email = dto.Email,
                MobilePhone = dto.MobilePhone,
                Question = dto.Question,
                Answer = dto.Answer,
                IsLock = dto.IsLock,
                LockBeginTime = dto.LockBeginTime,
                LockTime = dto.LockTime,
                LockMessage = dto.LockMessage,
                LastLoginTime = dto.LastLoginTime,
                LastLoginIp = dto.LastLoginIp,
                CurrentLoginTime = dto.CurrentLoginTime,
                CurrentLoginIp = dto.CurrentLoginIp,
                LoginTimes = dto.LoginTimes,
                LoginFailTimes = dto.LoginFailTimes,
                Note = dto.Note,
                Enabled = dto.Enabled,
                DisableTime = dto.DisableTime,
                CreateTime = dto.CreateTime,
                RegisterIp = dto.RegisterIp,
                Version = dto.Version,
            };
        }
        /// <summary>
        /// 转换为用户数据传输对象
        /// </summary>
        /// <param name="entity">用户实体</param>
        public static UserDto ToDto( this User entity ) {
            if( entity == null )
                return new UserDto();
            return new UserDto {
                Id = entity.Id.ToString(),
                TenantId = entity.TenantId,
                UserName = entity.UserName,
                Password = entity.Password,
                SafePassword = entity.SafePassword,
                Email = entity.Email,
                MobilePhone = entity.MobilePhone,
                Question = entity.Question,
                Answer = entity.Answer,
                IsLock = entity.IsLock,
                LockBeginTime = entity.LockBeginTime,
                LockTime = entity.LockTime,
                LockMessage = entity.LockMessage,
                LastLoginTime = entity.LastLoginTime,
                LastLoginIp = entity.LastLoginIp,
                CurrentLoginTime = entity.CurrentLoginTime,
                CurrentLoginIp = entity.CurrentLoginIp,
                LoginTimes = entity.LoginTimes,
                LoginFailTimes = entity.LoginFailTimes,
                Note = entity.Note,
                Enabled = entity.Enabled,
                DisableTime = entity.DisableTime,
                CreateTime = entity.CreateTime,
                RegisterIp = entity.RegisterIp,
                Version = entity.Version,
            };
        }
    }
}
