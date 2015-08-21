using Util.Datas.Ef;
using Applications.Domains.Models.Systems;
namespace Datas.Ef.Mappings.Systems
{
    public class ApplicationMap : AggregateMapBase<Application>
    {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable()
        {
            ToTable("Applications", "Systems");
        }

        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties()
        {
            //租户编号
            Property(t => t.Id)
                .HasColumnName("ApplicationId");
            //租户编码
            Property(t => t.Code)
                .HasColumnName("Code");
            //租户名称
            Property(t => t.Name)
                .HasColumnName("Name");
            //启用
            Property(t => t.Enabled)
                .HasColumnName("Enabled");
            //备注
            Property(t => t.Note)
                .HasColumnName("Note");
            //创建时间
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime");
        }
        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected override void MapAssociations()
        {
            HasMany(t => t.Tenants)
                    .WithMany(t => t.Applications)
                    .Map(m =>
                    {
                        m.ToTable("TenantInApplications", "Systems");
                        m.MapLeftKey("ApplicationId");
                        m.MapRightKey("TenantId");
                    });

        }
    }
}