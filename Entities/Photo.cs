using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Photo:Article
    {
        [DisplayName("内容")]
        [UIHint("Photo")]
        public string Text { get; set; }
    }
}
