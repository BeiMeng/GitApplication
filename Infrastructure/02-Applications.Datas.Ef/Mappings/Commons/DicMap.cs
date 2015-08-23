using Applications.Domains.Models.Commons;
using Util.Datas.Ef;

namespace Datas.Ef.Mappings.Commons {
    /// <summary>
    /// 字典映射
    /// </summary>
    public class DicMap : AggregateMapBase<Dic> {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable() {
            ToTable( "Dics", "Commons" );
        }

        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties() {
            //字典编号
            Property( t => t.Id )
                .HasColumnName( "DicId" );
            //租户编号
            Property( t => t.TenantId )
                .HasColumnName( "TenantId" );
            //父编号
            Property( t => t.ParentId )
                .HasColumnName( "ParentId" );
            //编码
            Property( t => t.Code )
                .HasColumnName( "Code" );
            //文本
            Property( t => t.Text )
                .HasColumnName( "Text" );
            //路径
            Property( t => t.Path )
                .HasColumnName( "Path" );
            //级数
            Property( t => t.Level )
                .HasColumnName( "Level" );
            //排序号
            Property( t => t.SortId )
                .HasColumnName( "SortId" );
            //拼音简码
            Property( t => t.PinYin )
                .HasColumnName( "PinYin" );
            //启用
            Property( t => t.Enabled )
                .HasColumnName( "Enabled" );
            //创建时间
            Property( t => t.CreateTime )
                .HasColumnName( "CreateTime" );
        }
        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected override void MapAssociations()
        {
            //租户
            HasRequired(t => t.Tenant)
                .WithMany(t => t.Dics)
                .HasForeignKey(d => d.TenantId);
        }
    }
}

