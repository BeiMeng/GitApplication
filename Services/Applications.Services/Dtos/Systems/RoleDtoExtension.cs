using System;
using System.Linq;
using Applications.Domains.Models.Systems;
using Util;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 角色数据传输对象扩展
    /// </summary>
    public static class RoleDtoExtension {
        /// <summary>
        /// 转换为角色实体
        /// </summary>
        /// <param name="dto">角色数据传输对象</param>
        public static Role ToEntity( this RoleDto dto ) {
            if( dto == null )
                return new Role();
            return new Role( dto.Id.ToGuid() ) {
                TenantId = dto.TenantId,
                Code = dto.Code,
                Name = dto.Name,
                Type = dto.Type,
                TypeName = dto.TypeName,
                IsAdmin = dto.IsAdmin,
                Note = dto.Note,
                PinYin = dto.PinYin,
                Enabled = dto.Enabled,
                CreateTime = dto.CreateTime,
                Version = dto.Version,
                Sign = dto.Sign,
            };
        }
        
        /// <summary>
        /// 转换为角色数据传输对象
        /// </summary>
        /// <param name="entity">角色实体</param>
        public static RoleDto ToDto( this Role entity ) {
            if( entity == null )
                return new RoleDto();
            return new RoleDto {
                Id = entity.Id.ToString(),
                TenantId = entity.TenantId,
                Code = entity.Code,
                Name = entity.Name,
                Type = entity.Type,
                TypeName = entity.TypeName,
                IsAdmin = entity.IsAdmin,
                Note = entity.Note,
                PinYin = entity.PinYin,
                Enabled = entity.Enabled,
                CreateTime = entity.CreateTime,
                Version = entity.Version,
                Sign = entity.Sign,
            };
        }
        /// <summary>
        /// 转换为角色数据传输对象
        /// </summary>
        /// <param name="entity">角色实体</param>
        /// <param name="userId">用户编号</param>
        public static RoleDto ToDto(this Role entity, Guid userId)
        {
            RoleDto dto = ToDto(entity);
            dto.Checked = entity.Users.Select(u => u.Id).Contains(userId);
            return dto;
        }
    }
}
