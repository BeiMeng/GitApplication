using Util.Exports;

namespace Util.Offices.Npoi {
    /// <summary>
    /// Npoi Excel2007 导出操作
    /// </summary>
    public class NpoiExcel2007Export : ExcelExport {
        /// <summary>
        /// 初始化Npoi Excel2003 导出操作
        /// </summary>
        public NpoiExcel2007Export() 
            : base ( ExportFormat.Xlsx, new NpoiExcel2007() ){
        }
    }
}
