
namespace Util.Algorithm {
    /// <summary>
    /// 排序
    /// </summary>
    public interface ISort {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="input">待排序数组</param>
        T[] Sort<T>( T[] input );
    }
}
