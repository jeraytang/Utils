using System;

namespace Utils
{
    public class DateTimeHelper
    {
         /// <summary>
        /// 输入的字符串是北京时间，转换为DateTimeOffset
        /// </summary>
        /// <param name="dateTime">北京时间</param>
        /// <returns></returns>
        public static DateTimeOffset GetBeiJinTime(string dateTime)
        {
            var date = DateTime.Parse(dateTime);
            // const string tzName = "China Standard Time";
            // var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzName);
            var timeZone = TimeZoneInfo.Local;
            var offset = timeZone.GetUtcOffset(date);
            return new DateTimeOffset(date, offset);
        }

        public static DateTimeOffset GetBeiJinTime(DateTime date)
        {
            var timeZone = TimeZoneInfo.Local;
            var offset = timeZone.GetUtcOffset(date);
            return new DateTimeOffset(date, offset);
        }

        /// <summary>
        /// 输入的值是utc时间，转换为DateTimeOffset
        /// </summary>
        /// <param name="dateTime">utc时间</param>
        /// <returns></returns>
        public static DateTimeOffset GetUtcTime(string dateTime)
        {
            var date = DateTime.Parse(dateTime);
            var timeZone = TimeZoneInfo.Utc;
            var offset = timeZone.GetUtcOffset(date);
            return new DateTimeOffset(date, offset);
        }

        public static DateTimeOffset GetUtcTime(DateTime date)
        {
            var dateTime = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var timeZone = TimeZoneInfo.Utc;
            var offset = timeZone.GetUtcOffset(dateTime);
            return new DateTimeOffset(dateTime, offset);
        }

        public static long GetTimestamp(DateTimeOffset dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// utc时区的DateTimeOffset，转为本地时区
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static DateTimeOffset UtcToLocalDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            //mysql里面是date类型，此处转换
            var timeZone = TimeZoneInfo.Utc;
            var dateTime = dateTimeOffset.ToLocalTime().DateTime;
            var offset = timeZone.GetUtcOffset(dateTimeOffset);
            var localDteTimeOffset = new DateTimeOffset(dateTime, offset);
            return localDteTimeOffset;
        }


        /// <summary>
        /// 获取月初、月末日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static (DateTime begin, DateTime end) GetMonthBeginAndEnd(DateTime date)
        {
            //上个月月初
            var monthBegin = date.AddDays(1 - date.Day).Date;
            //上个月月末
            var monthEnd = monthBegin.AddDays(1 - monthBegin.Day).Date.AddMonths(1).AddSeconds(-1);
            return (monthBegin, monthEnd);
        }

        /// <summary>
        /// 获取季度初、季度末日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static (DateTime begin, DateTime end) GetQuarterBeginAndEnd(DateTime date)
        {
            var quarterBegin = date.AddMonths(0 - (date.Month - 1) % 3).AddDays(1 - date.Day).Date;
            var quarterEnd = quarterBegin.AddMonths(3).AddSeconds(-1);
            return (quarterBegin, quarterEnd);
        }

        /// <summary>
        /// 获取季度的数字
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetQuarterOrder(DateTime date)
        {
            var month = date.Month;
            return month switch
            {
                1 => 1,
                2 => 1,
                3 => 1,

                4 => 2,
                5 => 2,
                6 => 2,

                7 => 3,
                8 => 3,
                9 => 3,

                10 => 4,
                11 => 4,
                12 => 4,
                _ => 0
            };
        }
    }
}