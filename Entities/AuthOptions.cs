using System;
namespace Entities
{
    public class AuthOptions
    {
        public string SecretKey { get; set; }
        public string CookieName { get; set; }
        public int ExpireDays { get; set; }
    }
}
