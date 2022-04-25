using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class MethodExt
    {
        public static V Get<T, V>(this Dictionary<T, V> dic, T key, V dft)
        {
            return dic.ContainsKey(key) ? dic[key] : dft;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> group, Action<int, T> action)
        {
            var i = 0;
            foreach (var item in group)
            {
                action(i, item);
                i++;
            }

            return group;
        }

        public static string JoinStr<T>(this IEnumerable<T> group, string separator)
        {
            return string.Join(separator, group.ToArray());
        }

        public static IEnumerable<T3> JoinByIndex<T, T2, T3>(this IEnumerable<T> group, IEnumerable<T2> otherGroup,
            Func<T, T2, T3> selectFunc)
        {
            return group.Select((item, i) =>
                selectFunc(item, otherGroup.Count() > i ? otherGroup.ElementAt(i) : default(T2))).ToList();
        }

        private static Random _Random = new Random();

        public static T Random<T>(this IEnumerable<T> group)
        {
            var index = _Random.Next(0, group.Count());
            return group.ElementAt(index);
        }

        public static HtmlNode QuerySelector2(this HtmlDocument doc, string selector)
        {
            return doc.DocumentNode.QuerySelectorAll2(selector).FirstOrDefault();
        }
        public static IEnumerable<HtmlNode> QuerySelectorAll2(this HtmlNode node, string selector)
        {
            #region 处理多个组合 section:contains('读书')||section:contains('读书'):next('section')
            if (selector.Contains("||"))
            {
                var orGroup = selector.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(item => node.QuerySelectorAll2(item))
                    .ToList();
                return orGroup;
            }
            #endregion

            #region 处理直接元素 .article-info>span .article-info>a span
            var match = Regex.Match(selector,
                @"(?<parent>[^>]+)>(?<child>\S+)(\s+(?<other>.*))?",
                RegexOptions.Multiline);
            if (match.Success)
            {
                var parentSelector = match.Groups["parent"].Value;
                var childSelector = match.Groups["child"].Value;
                var otherSelector = match.Groups["other"].Value;

                var parentGroup = node.QuerySelectorAll2(parentSelector);
                var childGroup = parentGroup.SelectMany(item => item.QuerySelectorAll2(childSelector)
                    .Where(sItem => sItem.ParentNode == item))
                    .ToList();
                var resultGroup = childGroup;

                if (!string.IsNullOrEmpty(otherSelector))
                {
                    resultGroup = childGroup.SelectMany(item => item.QuerySelectorAll2(otherSelector))
                        .ToList();
                }
                return resultGroup;
            }
            #endregion

            #region 处理包含文本 :contains('综合'):contains('高尔夫')
            match = Regex.Match(selector,
           @"(?<pre>.*?):contains\('(?<contains>.*?)'\)(?<aft>.*)",
           RegexOptions.Multiline);
            if (match.Success)
            {
                var preSelector = match.Groups["pre"].Value;
                var containSelector = match.Groups["contains"].Value;
                var aftSelector = match.Groups["aft"].Value;

                var group = new List<HtmlNode> { node };
                if (!string.IsNullOrEmpty(preSelector))
                {
                    group = node.QuerySelectorAll2(preSelector).ToList();
                }
                group = group.Where(item => item.InnerText.Contains(containSelector)).ToList();
                if (!string.IsNullOrEmpty(aftSelector))
                {
                    group = group.SelectMany(item => item.QuerySelectorAll2(aftSelector)).ToList();
                }
                return group;
            }
            #endregion

            #region 处理next section:contains('读书'):next('section')
            match = Regex.Match(selector,
             @"(?<pre>.*?):next\('(?<next>.*?)'\)(?<aft>.*)",
             RegexOptions.Multiline);
            if (match.Success)
            {
                var preSelector = match.Groups["pre"].Value;
                var nextSelector = match.Groups["next"].Value;
                var aftSelector = match.Groups["aft"].Value;

                var group = new List<HtmlNode> { node };
                if (!string.IsNullOrEmpty(preSelector))
                {
                    group = node.QuerySelectorAll2(preSelector).ToList();
                }

                group = group.Select(item =>
                {
                    var curIndex = item.ParentNode.ChildNodes.IndexOf(item);

                    var next = item.ParentNode.ChildNodes.Where((sItem, i) =>
                    {
                        return i > curIndex && sItem.QuerySelector(nextSelector) != null;
                    }).FirstOrDefault();

                    return next;
                })
                .Where(item => item != null)
                .ToList();

                if (!string.IsNullOrEmpty(aftSelector))
                {
                    group = group.SelectMany(item => item.QuerySelectorAll2(aftSelector)).ToList();
                }
                return group;
            }
            #endregion

            return node.QuerySelectorAll(selector)
           .Where(item => item.NodeType == HtmlNodeType.Element)
           .ToList();
        }
    }
}
