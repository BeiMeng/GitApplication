using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Util.Offices.Npoi {
    /// <summary>
    /// Npoi Excel2007操作
    /// </summary>
    public class NpoiExcel2007 : NpoiExcelBase {
        /// <summary>
        /// 创建工作薄
        /// </summary>
        protected override IWorkbook GetWorkbook() {
            return new XSSFWorkbook();
        }
    }
}
