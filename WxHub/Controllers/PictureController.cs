using System;
using System.IO;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OperateCenter.Controllers
{
    public class PictureController: Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public PictureController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpPost]
        public  ActionResult Upload(IFormFile fileData)
        {
            try
            {
                // 如果没有上传文件
                if (fileData == null ||
                    string.IsNullOrEmpty(fileData.FileName) ||
                    fileData.Length == 0)
                {
                    return Json(new { Success = false, Message = "请选择要上传的文件！" });
                    // return this.HttpNotFound();
                }
                Guid id = Guid.NewGuid();
                // 保存到 ~/photos 文件夹中，名称不变
                string fileName = System.IO.Path.GetFileName(fileData.FileName);
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                fileName = id.ToString() + fileExtension;

                string contentType = fileData.ContentType;
                if (String.IsNullOrEmpty(contentType))
                {
                    switch (fileExtension)
                    {
                        case ".bmp":
                            contentType = "image/bmp";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".jpeg":
                        case ".jpg":
                        case ".jpe":
                        case ".jfif":
                        case ".pjpeg":
                        case ".pjp":
                            contentType = "image/jpeg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".tiff":
                        case ".tif":
                            contentType = "image/tiff";
                            break;
                        default:
                            break;
                    }
                }
                string virtualPath =
                    string.Format("/images/thumbs/{0}", fileName);
                // 文件系统不能使用虚拟路径
                string webRootPath = _hostingEnvironment.WebRootPath;
                string path = webRootPath+virtualPath;
                //byte[] fileBinary = ConvertHelper.StreamToBytes(fileData.OpenReadStream());

                //fileData.SaveAs(path);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                     fileData.CopyTo(stream);
                }
                return Json(new { success = true, fileName = virtualPath, Id = id.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
