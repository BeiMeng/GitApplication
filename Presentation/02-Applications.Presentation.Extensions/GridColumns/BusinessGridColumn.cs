using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Util.Webs.EasyUi;
using Util.Webs.EasyUi.Grids;

namespace Presentation.Extensions.GridColumns {
    /// <summary>
    /// 表格列业务控件
    /// </summary>
    public partial class BusinessGridColumn<T> : IBusinessGridColumn<T> {
        /// <summary>
        /// 初始化表格列业务控件
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        public BusinessGridColumn( HtmlHelper<T> helper ) {
            _helper = helper;
        }

        /// <summary>
        /// HtmlHelper
        /// </summary>
        private readonly HtmlHelper<T> _helper;

        /// <summary>
        /// 字典控件
        /// </summary>
        /// <param name="expression">属性表达式</param>
        /// <param name="code">字典编码</param>
        public IDataGridColumn Dic<TProperty>(Expression<Func<T, TProperty>> expression, string code)
        {
            return EasyUiFactory<T>.CreateDataGridColumn(expression, _helper, true).ComboTree("/Commons/Dic/DicControl?code=" + code).Editable(false).Width(150).PanelHeight();
        }
        public IAreaGridColumn<T> Area()
        {
            return new AreaGridColumn<T>(_helper);
        }
    }
}
