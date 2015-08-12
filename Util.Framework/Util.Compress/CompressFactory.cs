namespace Util.Compress {
    /// <summary>
    /// 压缩工厂
    /// </summary>
    public class CompressFactory {
        /// <summary>
        /// 创建压缩器
        /// </summary>
        public static ICompress Create() {
            return CreateDotNetZip();
        }

        /// <summary>
        /// 创建Ionic压缩器
        /// </summary>
        public static ICompress CreateDotNetZip() {
            return new DotNetZip();
        }
    }
}
