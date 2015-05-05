using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationGlacier
{
    public class WitpUtility
    {
        public static string to_date_str(DateTime dt)
        {
            return string.Format("{0:00}{1:00}{2:00}", dt.Year - 1900, dt.Month, dt.Day);
        }
        public static DateTime from_date_str(string date_str)
        {
            var year = int.Parse(date_str.Substring(0,2)) + 1900;
            var month = int.Parse(date_str.Substring(2, 2));
            var day = int.Parse(date_str.Substring(4, 2));
            return new DateTime(year, month, day);
        }
    }
}