using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 列表扩展方法
    /// </summary>
    public static class ListExtensions
    {

        /// <summary>
        /// 拆分列表为指定大小的子列表
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="source">源列表</param>
        /// <param name="chunkSize">块元素数量</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            var result = source.Select((x, i) => new { Index = i, Value = x })

                                    .GroupBy(x => x.Index / chunkSize)

                                    .Select(x => x.Select(v => v.Value).ToList());
            return result;
        }
    }
}
