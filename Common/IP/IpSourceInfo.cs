using System;
using System.Collections.Generic;
using System.Text;

namespace Common.IP
{
    public class IpSourceInfo
    {
        public string Country { get; set; }
        public string Location { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string ProxyType { get; set; }
        public string ProtoType { get; set; }
        public double DownSpeed { get; set; }
        public double ConnectSpeed { get; set; }
        public double AliveDays { get; set; }
        public DateTime ValidateTime { get; set; }
    }
}
