using System;
using Util.Exports;

namespace Util.Offices.Npoi {
    /// <summary>
    /// 导出工厂
    /// </summary>
    public class ExportFactory : IExportFactory{
        /// <summary>
        /// 创建导出
        /// </summary>
        /// <param name="format">导出格式</param>
        public IExport Create( ExportFormat format ) {
            switch( format ) {
                case ExportFormat.Xlsx:
                    return CreateNpoiExcel2007Export();
                case ExportFormat.Xls:
                    return CreateNpoiExcel2003Export();
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建Npoi Excel 2003导出
        /// </summary>
        public static IExport CreateNpoiExcel2003Export() {
            return new NpoiExcel2003Export();
        }

        /// <summary>
        /// 创建Npoi Excel 2007导出
        /// </summary>
        public static IExport CreateNpoiExcel2007Export() {
            return new NpoiExcel2007Export();
        }
    }
}
