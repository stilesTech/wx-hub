using System;
namespace OperateCenter.Html
{
    public class HtmlTemplate
    {
       //public static string HeadBefore()
       // {
       //     string content= @"<mpprofile class='js_uneditable' data-pluginname='mpprofile' data-id='MzkzMzIwNjIwNQ==' data-headimg='http://mmbiz.qpic.cn/mmbiz_png/tjSQ2PSD8mOLZaEW6UicTzstAYCvryWCeSKicjcdRqWzKBlicyh2SCkEDicscyh0xwFURcnUBfRaNeVvPS7uPPLeyg/0?wx_fmt=png' data-nickname='小课课数学课堂' data-alias='' data-signature='免费提供优质1-9年级课堂同步学习资源，同步微课、在线练习、电子课本、口算天天练、一课一练、单元卷、期中期末卷等资源。' data-from='0'><div class='appmsg_card_context wx_profile_card js_wx_profile_card' data-id='MzkzMzIwNjIwNQ == ' data-isban='0' data-index='0'><div class='wx_profile_card_bd'><div class='wx_profile weui-flex'><div class='wx_profile_hd' style='width:44px;height:44px;' > <img  class='wx_profile_avatar' src='http://mmbiz.qpic.cn/mmbiz_png/tjSQ2PSD8mOLZaEW6UicTzstAYCvryWCeSKicjcdRqWzKBlicyh2SCkEDicscyh0xwFURcnUBfRaNeVvPS7uPPLeyg/0?wx_fmt=png' alt='小课课数学课堂'></div> <div class='wx_profile_bd weui-flex weui-flex__item'><div class='weui-flex__item'><strong class='wx_profile_nickname'>小课课数学课堂</strong><div class='wx_profile_desc'>免费提供优质1-9年级课堂同步学习资源，同步微课、在线练习、电子课本、口算天天练、一课一练、单元卷、期中期末卷等资源。</div><div class='wx_profile_tips' id='js_profile_desc'><span class='wx_profile_tips_meta' id='js_profile_article' style='display:none'>0篇原创内容</span><!-- <span class='wx_profile_tips_meta' id='js_profile_friends'></span> --></div></div><i class='weui-icon-arrow'></i></div></div></div><div class='wx_profile_card_ft'>公众号</div> </div></mpprofile>";
       //     return content;
       // }

        //public static string HeadBefore()
        //{
        //    string content = @"<mpprofile class='js_uneditable' data-pluginname='mpprofile' data-id='MzU4NjY1NzExNQ==' data-headimg='http://mmbiz.qpic.cn/mmbiz_png/u2vcdVpuicte9bsicbexMzunwkUTBK4pl2pgeGXibJfictxOPMtfl0uRrugRfoQfDC4AXPRHdwuH7mYorvc5juWAcA/0?wx_fmt=png' data-nickname='部编语文课本同步课堂' data-alias='momquan' data-signature='提供1-6年级学校同步课堂优质学习资源，名师课堂、电子课本、在线练习、学校考试资源、课外学习读物等；版本：部编版/人教版/北师大/牛津版/外研版/广东版等。' data-from='0'> <div class='appmsg_card_context wx_profile_card js_wx_profile_card' data-id='MzU4NjY1NzExNQ==' data-isban='0' data-index='0'>   <div class='wx_profile_card_bd'><div class='wx_profile weui-flex'><div class='wx_profile_hd'><img class='wx_profile_avatar' src='http://mmbiz.qpic.cn/mmbiz_png/u2vcdVpuicte9bsicbexMzunwkUTBK4pl2pgeGXibJfictxOPMtfl0uRrugRfoQfDC4AXPRHdwuH7mYorvc5juWAcA/0?wx_fmt=png' alt='部编语文课本同步课堂'></div><div class='wx_profile_bd weui-flex weui-flex__item'><div class='weui-flex__item'><strong class='wx_profile_nickname'>部编语文课本同步课堂</strong><div class='wx_profile_desc'>提供1-6年级学校同步课堂优质学习资源，名师课堂、电子课本、在线练习、学校考试资源、课外学习读物等；版本：部编版/人教版/北师大/牛津版/外研版/广东版等。</div><div class='wx_profile_tips' id='js_profile_desc'><span class='wx_profile_tips_meta' id='js_profile_article' style='display:none'>0篇原创内容</span><!-- <span class='wx_profile_tips_meta' id='js_profile_friends'></span> --></div></div><i class='weui-icon-arrow'></i></div></div></div><div class='wx_profile_card_ft'>公众号</div> </div> </mpprofile>";
        //    return content;
        //}

        //public static string HeadButton()
        //{
        //    string content = @"<div><p style='text-align: center;'><img class='rich_pages __bg_gif' data-galleryid='' data-ratio='0.5555555555555556' data-src='https://mmbiz.qpic.cn/mmbiz_gif/u2vcdVpuictdesJlkZeKvPEaicltq6ibHdj1jiaJA8HrUI0b1DMYloQW7Ngh0BqCLyvEickOWeX9s5d1GD5egThiaOoA/640?wx_fmt=gif ' data-type='gif' data-w='900' style='width: 677px !important; height: auto !important; visibility: visible !important;' _width='677px' src='https://mmbiz.qpic.cn/mmbiz_gif/u2vcdVpuictdesJlkZeKvPEaicltq6ibHdj1jiaJA8HrUI0b1DMYloQW7Ngh0BqCLyvEickOWeX9s5d1GD5egThiaOoA/640?wx_fmt=gif&tp=webp&wxfrom=5&wx_lazy=1' data-order='0' alt='图片' data-fail='0'></p></div>";
        //    return content;
        //}


        #region 目录页面
        public static string GetModule(string title)
        {
            return @"<section style='margin-top: 10px;margin-bottom: 10px;max-width: 100%;text-align: center;box-sizing: border-box !important;overflow-wrap: break-word !important;'><section style='margin-top: 15px;max-width: 100%;display: inline-block;box-sizing: border-box !important;overflow-wrap: break-word !important;'><p style='margin-right: 10px;margin-left: 10px;max-width: 100%;min-height: 1em;color: rgb(81, 81, 81);letter-spacing: 5px;box-sizing: border-box !important;overflow-wrap: break-word !important;'><span style='max-width: 100%;font-size: 18px;color: rgb(105, 158, 108);letter-spacing: normal;box-sizing: border-box !important;overflow-wrap: break-word !important;'><strong style='max-width: 100%;box-sizing: border-box !important;overflow-wrap: break-word !important;'>"+title+"</strong></span></p><section style='margin-top: -10px;max-width: 100%;background-image: -webkit-linear-gradient(left, rgb(255, 238, 225), rgb(255, 238, 225), rgb(219, 246, 206));height: 12px;box-sizing: border-box !important;overflow-wrap: break-word !important;'><br></section></section></section>";
        }


        public static string GetLinkTitle(string title, string linkUrl)
        {
            return @"<section style='line-height: 1.75em;'><a target='_blank' href='"+linkUrl+"' data-itemshowtype='0' data-linktype='2' style='font-size: 16px;letter-spacing: normal;' hasload='1'><span style='font-size: 16px;letter-spacing: normal;'>"+title+"</span></a><br></section>";
        }

        public static string GetSubModule(string title)
        {
            return @"<p style='max-width: 100%;min-height: 1em;font-family: -apple-system-font, BlinkMacSystemFont, &quot;Helvetica Neue&quot;, &quot;PingFang SC&quot;, &quot;Hiragino Sans GB&quot;, &quot;Microsoft YaHei UI&quot;, &quot;Microsoft YaHei&quot;, Arial, sans-serif;letter-spacing: 0.544px;white-space: normal;background-color: rgb(255, 255, 255);line-height: 1.75em;box-sizing: border-box !important;overflow-wrap: break-word !important;'><span style='max-width: 100%;color: rgb(217, 33, 66);font-size: 18px;letter-spacing: normal;box-sizing: border-box !important;overflow-wrap: break-word !important;'>"+title+"</span></p>";
        }
        #endregion

    }
}
