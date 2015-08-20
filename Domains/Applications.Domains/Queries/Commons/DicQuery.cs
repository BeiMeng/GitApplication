using System;
using System.ComponentModel.DataAnnotations;
using Util.Domains;

namespace Applications.Domains.Queries.Commons {
    /// <summary>
    /// 字典查询实体
    /// </summary>
    public class DicQuery : TreeEntityQuery
    {
        /// <summary>
        /// 字典编号
        /// </summary>
        [Display( Name = "字典编号" )]
        public Guid? DicId { get; set; }

        private string _code = string.Empty;
        /// <summary>
        /// 编码
        /// </summary>
        [Display( Name = "编码" )]
        public string Code {
            get { return _code == null ? string.Empty : _code.Trim(); }
            set { _code = value; }
        }

        private string _text = string.Empty;
        /// <summary>
        /// 文本
        /// </summary>
        [Display( Name = "文本" )]
        public string Text {
            get { return _text == null ? string.Empty : _text.Trim(); }
            set { _text = value; }
        }

        private string _pinYin = string.Empty;
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Display( Name = "拼音简码" )]
        public string PinYin {
            get { return _pinYin == null ? string.Empty : _pinYin.Trim(); }
            set { _pinYin = value; }
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
            AddDescription( "字典编号", DicId );
            AddDescription( "编码", Code );
            AddDescription( "文本", Text );
            AddDescription( "拼音简码", PinYin );
            AddDescription( "起始创建时间", BeginCreateTime );
            AddDescription( "结束创建时间", EndCreateTime );
        }

        /// <summary>
        /// 检查查询参数是否全部为空值
        /// </summary>
        protected override void CheckEmpty() {
            base.CheckEmpty();
            AddValue( DicId );
            AddValue( Code );
            AddValue( Text );
            AddValue( PinYin );
            AddValue( BeginCreateTime );
            AddValue( EndCreateTime );
        }
    }
}
