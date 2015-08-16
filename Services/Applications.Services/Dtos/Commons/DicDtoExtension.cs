using Applications.Domains.Models.Commons;
using Util;

namespace Applications.Services.Dtos.Commons {
    /// <summary>
    /// 字典数据传输对象扩展
    /// </summary>
    public static class DicDtoExtension {
        /// <summary>
        /// 转换为字典实体
        /// </summary>
        /// <param name="dto">字典数据传输对象</param>
        public static Dic ToEntity( this DicDto dto ) {
            return new Dic( dto.Id.ToGuid(),dto.Path,dto.Level.SafeValue() ) {
                ParentId = dto.ParentId.ToGuidOrNull(),
                TenantId = dto.TenantId,
                Code = dto.Code,
                Text = dto.Text,
                SortId = dto.SortId,
                PinYin = dto.PinYin,
                Enabled = dto.Enabled,
                CreateTime = dto.CreateTime,
                Version = dto.Version,
            };
        }

        /// <summary>
        /// 转换为字典数据传输对象
        /// </summary>
        /// <param name="entity">字典实体</param>
        public static DicDto ToDto( this Dic entity ) {
            return new DicDto {
                Id = entity.Id.ToString(),
                TenantId = entity.TenantId,
                ParentId = entity.ParentId.ToStr(),
                Code = entity.Code,
                Text = entity.Text,
                Path = entity.Path,
                Level = entity.Level,
                SortId = entity.SortId,
                PinYin = entity.PinYin,
                Enabled = entity.Enabled,
                CreateTime = entity.CreateTime,
                Version = entity.Version,
            };
        }
    }
}
