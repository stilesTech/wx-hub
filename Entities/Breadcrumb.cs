using System;
namespace Entities
{
    public class Breadcrumb
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Breadcrumb childen { get; set; }
    }
}
