using System;

namespace Util.Algorithm {
    /// <summary>
    /// 冒泡排序算法
    /// </summary>
    public class BubbleSort : ISort {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="input">待排序数组</param>
        public T[] Sort<T>( T[] input ) {
            for ( var i = 1; i < input.Length; i++ ) {
                var key = input[i];
                var j = i - 1;
                while ( j >= 0 && ( (IComparable)input[j] ).CompareTo( key ) > 0 ) {
                    input[j + 1] = input[j];
                    j = j - 1;
                }
                input[j + 1] = key;
            }
            return input;
        }
    }
}
