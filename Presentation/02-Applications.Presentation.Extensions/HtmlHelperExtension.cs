using System.Web.Mvc;

namespace Presentation.Extensions {
    /// <summary>
    /// HtmlHelper扩展
    /// </summary>
    public static class HtmlHelperExtension {
        /// <summary>
        /// 业务控件
        /// </summary>
        public static IBusinessControl<T> Ui<T>( this HtmlHelper<T> helper ) {
            return new BusinessControl<T>( helper );
        }
    }
}
