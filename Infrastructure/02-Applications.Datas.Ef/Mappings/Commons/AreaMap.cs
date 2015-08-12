using Applications.Domains.Models.Commons;
using Util.Datas.Ef;

namespace Datas.Ef.Mappings.Commons {
    /// <summary>
    /// 地区映射
    /// </summary>
    public class AreaMap : AggregateMapBase<Area> {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable() {
            ToTable( "Areas", "Commons" );
        }
        
        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties() {
            //映射基类属性
            base.MapProperties();
            //地区编号
            Property(t => t.Id)
                .HasColumnName("AreaId");
            //父编号
            Property(t => t.ParentId)
                .HasColumnName("ParentId");
            //编码
            Property(t => t.Code)
                .HasColumnName("Code");
            //地区名称
            Property(t => t.Text)
                .HasColumnName("Text");
            //路径
            Property(t => t.Path)
                .HasColumnName("Path");
            //级数
            Property(t => t.Level)
                .HasColumnName("Level");
            //排序号
            Property(t => t.SortId)
                .HasColumnName("SortId");
            //拼音简码
            Property(t => t.PinYin)
                .HasColumnName("PinYin");
            //全拼
            Property(t => t.FullPinYin)
                .HasColumnName("FullPinYin");
            //启用
            Property(t => t.Enabled)
                .HasColumnName("Enabled");
            //创建时间
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime");
        }
        
        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected override void MapAssociations() {            
        }
    }
}

