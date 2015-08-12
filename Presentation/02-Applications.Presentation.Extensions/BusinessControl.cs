using System.Web.Mvc;
using Presentation.Extensions.GridColumns;

namespace Presentation.Extensions {
    /// <summary>
    /// 业务控件
    /// </summary>
    public partial class BusinessControl<T> : IBusinessControl<T> {
        /// <summary>
        /// 初始化业务控件
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        public BusinessControl( HtmlHelper<T> helper ) {
            _helper = helper;
        }

        /// <summary>
        /// HtmlHelper
        /// </summary>
        private readonly HtmlHelper<T> _helper;

        /// <summary>
        /// 表格列业务控件
        /// </summary>
        public IBusinessGridColumn<T> GridColumn() {
            return new BusinessGridColumn<T>( _helper );
        }
    }
}
