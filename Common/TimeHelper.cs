using System;
using Microsoft.Extensions.Internal;
using NodaTime;

namespace Common
{
    public class TimeHelper
    {
        public static DateTime GetNow()
        {
            return GetCstDateTime();
        }

        public static DateTime GetCstDateTime()
        {
            Instant now = NodaTime.SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }
    }
 
}
