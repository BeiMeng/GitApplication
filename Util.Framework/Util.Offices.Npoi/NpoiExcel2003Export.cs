using Util.Exports;

namespace Util.Offices.Npoi {
    /// <summary>
    /// Npoi Excel2003 导出操作
    /// </summary>
    public class NpoiExcel2003Export : ExcelExport {
        /// <summary>
        /// 初始化Npoi Excel2003 导出操作
        /// </summary>
        public NpoiExcel2003Export() 
            : base ( ExportFormat.Xls, new NpoiExcel2003() ){
        }
    }
}
