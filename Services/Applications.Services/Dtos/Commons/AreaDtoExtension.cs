using Applications.Domains.Models.Commons;
using Util;

namespace Applications.Services.Dtos.Commons {
    /// <summary>
    /// 地区数据传输对象扩展
    /// </summary>
    public static class AreaDtoExtension {
        /// <summary>
        /// 转换为地区实体
        /// </summary>
        /// <param name="dto">地区数据传输对象</param>
        public static Area ToEntity( this AreaDto dto ) {
            return new Area( dto.Id.ToGuid(),dto.Path,dto.Level.SafeValue() ) {
                ParentId = dto.ParentId.ToGuidOrNull(),
                Code = dto.Code,
                Text = dto.Text,
                SortId = dto.SortId,
                PinYin = dto.PinYin,
                FullPinYin = dto.FullPinYin,
                Enabled = dto.Enabled,
                CreateTime = dto.CreateTime,
                Version = dto.Version,
            };
        }

        /// <summary>
        /// 转换为地区数据传输对象
        /// </summary>
        /// <param name="entity">地区实体</param>
        public static AreaDto ToDto( this Area entity ) {
            return new AreaDto {
                Id = entity.Id.ToString(),
                ParentId = entity.ParentId.ToStr(),
                Code = entity.Code,
                Text = entity.Text,
                Path = entity.Path,
                Level = entity.Level,
                SortId = entity.SortId,
                PinYin = entity.PinYin,
                FullPinYin = entity.FullPinYin,
                Enabled = entity.Enabled,
                CreateTime = entity.CreateTime,
                Version = entity.Version,
            };
        }
    }
}
