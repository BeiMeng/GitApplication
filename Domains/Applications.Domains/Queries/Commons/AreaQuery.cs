using System;
using System.ComponentModel.DataAnnotations;
using Util.Domains;

namespace Applications.Domains.Queries.Commons {
    /// <summary>
    /// 地区查询实体
    /// </summary>
    public class AreaQuery : TreeEntityQuery{        
        /// <summary>
        /// 地区编号
        /// </summary>
        [Display(Name="地区编号")]
        public Guid? AreaId { get; set; }
        
        private string _code = string.Empty; 
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name="编码")]
        public string Code {
            get { return _code == null ? string.Empty : _code.Trim(); }
            set{ _code=value;}
        }
        
        private string _text = string.Empty; 
        /// <summary>
        /// 地区名称
        /// </summary>
        [Display(Name="地区名称")]
        public string Text {
            get { return _text == null ? string.Empty : _text.Trim(); }
            set{ _text=value;}
        }
        
        private string _pinYin = string.Empty; 
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Display(Name="拼音简码")]
        public string PinYin {
            get { return _pinYin == null ? string.Empty : _pinYin.Trim(); }
            set{ _pinYin=value;}
        }
        
        private string _fullPinYin = string.Empty; 
        /// <summary>
        /// 全拼
        /// </summary>
        [Display(Name="全拼")]
        public string FullPinYin {
            get { return _fullPinYin == null ? string.Empty : _fullPinYin.Trim(); }
            set{ _fullPinYin=value;}
        }
        
        /// <summary>
        /// 起始创建时间
        /// </summary>
        [Display( Name = "起始创建时间" )]
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 结束创建时间
        /// </summary>
        [Display( Name = "结束创建时间" )]
        public DateTime? EndCreateTime { get; set; }
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            base.AddDescriptions();
            AddDescription( "地区编号", AreaId ); 
            AddDescription( "编码", Code ); 
            AddDescription( "地区名称", Text ); 
            AddDescription( "拼音简码", PinYin ); 
            AddDescription( "全拼", FullPinYin ); 
            AddDescription( "起始创建时间", BeginCreateTime );
            AddDescription( "结束创建时间", EndCreateTime );
        }

        /// <summary>
        /// 检查查询参数是否全部为空值
        /// </summary>
        protected override void CheckEmpty() {
            base.CheckEmpty();
            AddValue( AreaId );
            AddValue( Code );
            AddValue( Text );
            AddValue( PinYin );
            AddValue( FullPinYin );
            AddValue( BeginCreateTime );
            AddValue( EndCreateTime );
        }
    }
}
