using System;
using System.ComponentModel.DataAnnotations;
using Util;
using Util.Domains;

namespace Applications.Domains.Models.Commons {
    /// <summary>
    /// 地区
    /// </summary>
    public partial class Area : TreeEntityBase<Area> {
        /// <summary>
        /// 初始化地区
        /// </summary>
        public Area()
            : this( Guid.Empty, "", 0 ) {
        }

        /// <summary>
        /// 初始化地区
        /// </summary>
        /// <param name="id">地区标识</param>
        /// <param name="path">路径</param>
        /// <param name="level">级数</param>
        public Area( Guid id, string path, int level )
            : base( id, path, level ) {
        }

        /// <summary>
        /// 编码
        /// </summary>
        [Required( ErrorMessage = "编码不能为空" )]
        [StringLength( 10, ErrorMessage = "编码输入过长，不能超过10位" )]
        public string Code { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        [Required( ErrorMessage = "地区名称不能为空" )]
        [StringLength( 200, ErrorMessage = "地区名称输入过长，不能超过200位" )]
        public string Text { get; set; }
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Required( ErrorMessage = "拼音简码不能为空" )]
        [StringLength( 200, ErrorMessage = "拼音简码输入过长，不能超过200位" )]
        public string PinYin { get; set; }
        /// <summary>
        /// 全拼
        /// </summary>
        [StringLength( 500, ErrorMessage = "全拼输入过长，不能超过500位" )]
        public string FullPinYin { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required( ErrorMessage = "创建时间不能为空" )]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            AddDescription( "地区编号", Id );
            AddDescription( "父编号", ParentId );
            AddDescription( "编码", Code );
            AddDescription( "地区名称", Text );
            AddDescription( "路径", Path );
            AddDescription( "级数", Level );
            AddDescription( "排序号", SortId );
            AddDescription( "拼音简码", PinYin );
            AddDescription( "全拼", FullPinYin );
            AddDescription( "启用", Enabled.Description() );
            AddDescription( "创建时间", CreateTime );
        }
    }
}