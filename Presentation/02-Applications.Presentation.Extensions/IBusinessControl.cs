namespace Presentation.Extensions {
    /// <summary>
    /// 业务控件
    /// </summary>
    public interface IBusinessControl<T> {
        /// <summary>
        /// 表格列业务控件
        /// </summary>
        IBusinessGridColumn<T> GridColumn();
    }
}
