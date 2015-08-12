using System;
using System.Linq.Expressions;
using Presentation.Extensions.GridColumns;
using Util.Webs.EasyUi.Grids;

namespace Presentation.Extensions {
    /// <summary>
    /// 表格列业务控件
    /// </summary>
    public interface IBusinessGridColumn<T> {
        /// <summary>
        /// 字典控件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">属性表达式</param>
        /// <param name="code">字典编码</param>
        IDataGridColumn Dic<TProperty>( Expression<Func<T, TProperty>> expression, string code );
        /// <summary>
        /// 地区控件
        /// </summary>
        IAreaGridColumn<T> Area();
    }
}
