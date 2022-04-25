using Entities.Query;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperateCenter.Extensions
{
    public static class PagerHtmlExtensions
    {
        public static IHtmlContent Pager(this IHtmlHelper html, PagerQuery model)
        {
            string htmlstr = string.Empty;
            string htmlstr1 = string.Empty;
            if (model.PageCounts <= model.PageSize)
            {
                for (int i = 1; i < model.PageCounts + 1; i++)
                {
                    if (i == model.PageIndex)
                    {
                        htmlstr1 += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\">" + i + "</a>";
                    }
                    else
                    {
                        htmlstr1 += "<a href=\"?pg=" + i + model.Key + "\" class=\"btn\">" + i + "</a>";
                    }
                }
            }
            else if (model.PageIndex <= 4)
            {
                for (int i = 1; i < 11; i++)
                {
                    if (i == model.PageIndex)
                    {
                        htmlstr1 += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\">" + i + "</a>";
                    }
                    else
                    {
                        htmlstr1 += "<a href=\"?pg=" + i + model.Key + "\" class=\"btn\">" + i + "</a>";
                    }
                }
            }
            else if (model.PageIndex > model.PageCounts - 5)
            {
                for (int i = model.PageIndex - (model.PageIndex - (model.PageCounts - 5) + 4); i < model.PageCounts + 1; i++)
                {
                    if (i == model.PageIndex)
                    {
                        htmlstr1 += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\">" + i + "</a>";
                    }
                    else
                    {
                        htmlstr1 += "<a href=\"?pg=" + i + model.Key + "\" class=\"btn\">" + i + "</a>";
                    }
                }
            }
            else
            {
                for (int i = model.PageIndex - 4; i < model.PageIndex + 6; i++)
                {
                    if (i == model.PageIndex)
                    {
                        htmlstr1 += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\">" + i + "</a>";
                    }
                    else
                    {
                        htmlstr1 += "<a href=\"?pg=" + i + model.Key + "\" class=\"btn\">" + i + "</a>";
                    }
                }
            }
            htmlstr = "<div class=\"btn-group pull-left\">";
            if (model.PageIndex == 0 || model.PageIndex == 1)
            {
                htmlstr += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\"><i class=\"icon-home\"></i>首页</a>" +
                    "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\"><i class=\"icon-step-backward\"></i>上一页</a>";
            }
            else
            {
                htmlstr += "<a href=\"?pg=1"+model.Key+"\" class=\"btn\"><i class=\"icon-home\"></i>首页</a>" +
                    "<a href=\"?pg=" + (model.PageIndex - 1) + model.Key + "\" class=\"btn\"><i class=\"icon-step-backward\"></i>上一页</a>";
            }
            htmlstr += htmlstr1;
            if (model.PageIndex == model.PageCounts)
            {
                htmlstr += "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\"><i class=\"icon-step-forward\"></i>下一页</a>" +
                "<a href=\"javascript:void(0);\" class=\"btn\" disabled=\"disabled\"><i class=\"icon-share-alt\"></i>尾页</a></div>";
            }
            else
            {
                htmlstr += "<a href=\"?pg=" + (model.PageIndex + 1) + model.Key + "\" class=\"btn\"><i class=\"icon-step-forward\"></i>下一页</a>" +
                "<a href=\"?pg=" + model.PageCounts+model.Key + "\" class=\"btn\"><i class=\"icon-share-alt\"></i>尾页</a></div>";
            }
             return new HtmlString(htmlstr);
        }
    }
}
