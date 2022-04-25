using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    [Serializable]
    public partial class Picture
    {
        #region Model
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID不能为空")]
        public Guid PId
        {
            get;
            set;
        }

        [DisplayName("图片编码")]
        public byte[] Binary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("图片类型")]
        [Required(ErrorMessage = "图片类型不能为空")]
        public string MimeType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("文件名称")]
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("是否新建")]
        public bool IsNew
        {
            get;
            set;
        }

        #endregion Model

    }
}
