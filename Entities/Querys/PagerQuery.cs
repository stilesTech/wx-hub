using System;
namespace Entities.Query
{
    public class PagerQuery
    {
        /// <summary>
        /// 分页模型
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="key">查询关键字(例如 &key=value)</param>
        /// <param name="pageCounts">页面总数</param>
        /// <param name="recordCounts">记录总数</param>
        public PagerQuery(int pageIndex = 1, int pageSize = 10, string key = "", int pageCounts = 0, int recordCounts = 0)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Key = key;
            this.PageCounts = pageCounts;
            this.RecordCounts = recordCounts;
        }
        #region Properties

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页尺寸
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页面总数
        /// </summary>
        public int PageCounts { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCounts { get; set; }
        /// <summary>
        /// 点击页码跳转到Url页面
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Key { get; set; }
        #endregion Properties
    }
}
