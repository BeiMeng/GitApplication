using System;
using System.Collections.Generic;
using System.Linq;

namespace Util {
    public static partial class Extensions {
        /// <summary>
        /// 过滤重复项
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TKey">重复属性类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="keySelector">过滤表达式</param>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
            var keys = new HashSet<TKey>();
            return source.Where( element => keys.Add( keySelector( element ) ) );
        }
    }
}
