using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Util.Offices.Npoi {
    /// <summary>
    /// Npoi Excel2003操作
    /// </summary>
    public class NpoiExcel2003 : NpoiExcelBase {
        /// <summary>
        /// 创建工作薄
        /// </summary>
        protected override IWorkbook GetWorkbook() {
            return new HSSFWorkbook();
        }
    }
}
