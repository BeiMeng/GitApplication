using Applications.Domains.Models.Systems;
using Util;

namespace Applications.Services.Dtos.Systems {
    /// <summary>
    /// 租户数据传输对象扩展
    /// </summary>
    public static class TenantDtoExtension {
        /// <summary>
        /// 转换为租户实体
        /// </summary>
        /// <param name="dto">租户数据传输对象</param>
        public static Tenant ToEntity( this TenantDto dto ) {
            if( dto == null )
                return new Tenant();
            return new Tenant(dto.Id.ToGuid(), dto.Path, dto.Level.SafeValue())
            {
                Code = dto.Code,
                Name = dto.Text,
                ParentId = dto.ParentId.ToGuidOrNull(),
                ContactName = dto.ContactName,
                Email = dto.Email,
                Phone = dto.Phone,
                MobilePhone = dto.MobilePhone,
                Fax = dto.Fax,
                Qq = dto.Qq,
                ProvinceId = dto.ProvinceId,
                Province = dto.Province,
                CityId = dto.CityId,
                City = dto.City,
                CountyId = dto.CountyId,
                County = dto.County,
                Street = dto.Street,
                Zip = dto.Zip,
                Enabled = dto.Enabled,
                TState = dto.TState,
                SortId = dto.SortId,
                PinYin = dto.PinYin,
                Note = dto.Note,
                CreateTime = dto.CreateTime,
                Version = dto.Version,
            };
        }
        
        ///// <summary>
        ///// 转换为租户实体
        ///// </summary>
        ///// <param name="dto">租户数据传输对象</param>
        //public static Tenant ToEntity2( this TenantDto dto ) {
        //    if( dto == null )
        //        return new Tenant();
        //    return TenantFactory.Create(
        //        tenantId : dto.Id.ToGuid(),
        //        code : dto.Code,
        //        name : dto.Name,
        //        parentId : dto.ParentId,
        //        path : dto.Path,
        //        level : dto.Level,
        //        contactName : dto.ContactName,
        //        email : dto.Email,
        //        phone : dto.Phone,
        //        mobilePhone : dto.MobilePhone,
        //        fax : dto.Fax,
        //        qq : dto.Qq,
        //        provinceId : dto.ProvinceId,
        //        province : dto.Province,
        //        cityId : dto.CityId,
        //        city : dto.City,
        //        countyId : dto.CountyId,
        //        county : dto.County,
        //        street : dto.Street,
        //        zip : dto.Zip,
        //        enabled : dto.Enabled,
        //        state : dto.State,
        //        sortId : dto.SortId,
        //        pinYin : dto.PinYin,
        //        note : dto.Note,
        //        createTime : dto.CreateTime,
        //        version : dto.Version
        //    );
        //}

        /// <summary>
        /// 转换为租户数据传输对象
        /// </summary>
        /// <param name="entity">租户实体</param>
        public static TenantDto ToDto( this Tenant entity ) {
            if( entity == null )
                return new TenantDto();
            return new TenantDto {
                Id = entity.Id.ToString(),
                Code = entity.Code,
                Text = entity.Name,
                ParentId = entity.ParentId.ToStr(),
                Path = entity.Path,
                Level = entity.Level,
                ContactName = entity.ContactName,
                Email = entity.Email,
                Phone = entity.Phone,
                MobilePhone = entity.MobilePhone,
                Fax = entity.Fax,
                Qq = entity.Qq,
                ProvinceId = entity.ProvinceId,
                Province = entity.Province,
                CityId = entity.CityId,
                City = entity.City,
                CountyId = entity.CountyId,
                County = entity.County,
                Street = entity.Street,
                Zip = entity.Zip,
                Enabled = entity.Enabled,
                TState = entity.TState,
                SortId = entity.SortId,
                PinYin = entity.PinYin,
                Note = entity.Note,
                CreateTime = entity.CreateTime,
                Version = entity.Version,
            };
        }
    }
}
