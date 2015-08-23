using Applications.Domains.Models.Systems;
using Util.Datas.Ef;

namespace Datas.Ef.Mappings.Systems {
    /// <summary>
    /// 角色映射
    /// </summary>
    public class RoleMap : AggregateMapBase<Role> {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable() {
            ToTable( "Roles", "Systems" );
        }
        
        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties() {
            //角色编号
            Property(t => t.Id)
                .HasColumnName("RoleId");
            //租户编号
            Property(t => t.TenantId)
                .HasColumnName("TenantId");
            //角色编码
            Property(t => t.Code)
                .HasColumnName("Code");
            //角色名称
            Property(t => t.Name)
                .HasColumnName("Name");
            //角色类型
            Property(t => t.Type)
                .HasColumnName("Type");
            //角色类型名称
            Property(t => t.TypeName)
                .HasColumnName("TypeName");
            //管理员
            Property(t => t.IsAdmin)
                .HasColumnName("IsAdmin");
            //备注
            Property(t => t.Note)
                .HasColumnName("Note");
            //拼音简码
            Property(t => t.PinYin)
                .HasColumnName("PinYin");
            //启用
            Property(t => t.Enabled)
                .HasColumnName("Enabled");
            //创建时间
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime");
            //签名
            Property(t => t.Sign)
                .HasColumnName("Sign");
        }
        
        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected override void MapAssociations() {
            //租户
            HasRequired(t => t.Tenant)
                .WithMany(t => t.Roles)
                .HasForeignKey(d => d.TenantId);
            //用户
            HasMany(t => t.Users)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("UsersInRoles", "Systems");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("UserId");
                });
        }
    }
}
