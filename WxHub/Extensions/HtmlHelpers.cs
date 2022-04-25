using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OperateCenter.Extensions
{
    public　static class HtmlHelpers
    {
        public static string Thumbnail(this IHtmlHelper helper, string imgSrc, string size)
        {
            var imgsrc = imgSrc;
            var imgpath = imgsrc.Remove(imgsrc.LastIndexOf("/", StringComparison.Ordinal));
            var imgname = imgsrc.Remove(0, imgsrc.LastIndexOf("/", StringComparison.Ordinal) + 1);
            imgsrc = imgpath + "/" + size + "/" + imgname;
            return imgsrc;
        }
        public static string JudgeSingularOrPlural(this HtmlHelper helper, int count, string singularKey, string pluralKey)
        {
            return count > 1 ? pluralKey : singularKey;
        }
        public static string GetDate(this HtmlHelper helper, DateTime? dt, string formart = "")
        {
            if (dt == null)
            {
                return string.Empty;
            }
            var date = Convert.ToDateTime(dt);
            if (formart != "")
            {
                return date.ToString(formart);
            }
            return date.ToString("yyyy-MM-dd");
        }


        public static IHtmlContent Breadcrumb(this IHtmlHelper html,Breadcrumb model){
            if(model==null) return null;
            string htmlstr= string.Empty;
           htmlstr="<div class=\"span12\"><ul class=\"breadcrumb\">";
           htmlstr+=string.Format("<li><a href=\"{0}\">{1}</a> <span class=\"divider\">/</span></li>",model.Url,model.Title);
                   if(model.childen!=null){
                        htmlstr+=string.Format("<li><a href=\"{0}\">{1}</a> <span class=\"divider\">/</span></li>",model.childen.Url,model.childen.Title);
                        if (model.childen.childen != null)
                            htmlstr += string.Format("<li class=\"active\">{0}</li>", model.childen.childen.Title);
                   }     
               htmlstr+="</ul></div>";
            
             return new HtmlString(htmlstr);
        }
    }
}
