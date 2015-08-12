using Applications.Domains.Models.Systems;
using Util.Datas.Ef;

namespace Datas.Ef.Mappings.Systems {
    /// <summary>
    /// 租户映射
    /// </summary>
    public class TenantMap : AggregateMapBase<Tenant> {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable() {
            ToTable( "Tenants", "Systems" );
        }
        
        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties() {
            //租户编号
            Property( t => t.Id )
                .HasColumnName( "TenantId" );
            //租户编码
            Property(t => t.Code)
                .HasColumnName("Code");
            //租户名称
            Property(t => t.Name)
                .HasColumnName("Name");
            //父编号
            Property(t => t.ParentId)
                .HasColumnName("ParentId");
            //路径
            Property(t => t.Path)
                .HasColumnName("Path");
            //级数
            Property(t => t.Level)
                .HasColumnName("Level");
            //联系人姓名
            Property(t => t.ContactName)
                .HasColumnName("ContactName");
            //电子邮件
            Property( t => t.Email )
                .HasColumnName( "Email" );
            //电话
            Property( t => t.Phone )
                .HasColumnName( "Phone" );
            //手机
            Property( t => t.MobilePhone )
                .HasColumnName( "MobilePhone" );
            //传真
            Property( t => t.Fax )
                .HasColumnName( "Fax" );
            //Qq
            Property( t => t.Qq )
                .HasColumnName( "Qq" );
            //状态
            Property(t => t.TState)
                .HasColumnName("TState");
            //启用
            Property(t => t.Enabled)
                .HasColumnName("Enabled");
            //备注
            Property(t => t.Note)
                .HasColumnName("Note");
            //排序号
            Property( t => t.SortId )
                .HasColumnName( "SortId" );
            //拼音简码
            Property(t => t.PinYin)
                .HasColumnName("PinYin");
            //创建时间
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime");
        }
    }
}

