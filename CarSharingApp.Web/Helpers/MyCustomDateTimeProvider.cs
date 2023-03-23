using System.Globalization;

namespace CarSharingApp.Web.Helpers
{
    public static class MyCustomDateTimeProvider
    {
        public static DateTime ProvideCurrentUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime ProvideCurrentCustomerLocalDateTime()
        {
            return DateTime.Now;
        }

        public static DateTime ParseFromViewIntoCurrentCustomerLocalDateTime(string localDateTime)
        {
            // input format is "ddd MMM dd yyyy HH:mm:ss GMT +0300" or "ddd MMM dd yyyy HH:mm:ss"

            string circumcisedDateTimeStr;

            if (localDateTime.IndexOf("GMT") is -1 or 0)
            {
                circumcisedDateTimeStr = localDateTime;
            }
            else
            {
                circumcisedDateTimeStr = localDateTime.Substring(0, localDateTime.IndexOf("GMT") - 1);
            }

            DateTime result = DateTime.ParseExact(circumcisedDateTimeStr, "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return result;
        }

        public static DateTime ParseFromLocalToUtcDateTime(string localDateTime)
        {
            // input format is "ddd MMM dd yyyy HH:mm:ss GMT +0300" or "ddd MMM dd yyyy HH:mm:ss"

            string circumcisedDateTimeStr;

            if (localDateTime.IndexOf("GMT") is -1 or 0)
            {
                circumcisedDateTimeStr = localDateTime;
            }
            else
            {
                circumcisedDateTimeStr = localDateTime.Substring(0, localDateTime.IndexOf("GMT") - 1);
            }

            DateTime result = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(circumcisedDateTimeStr, "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture), TimeZoneInfo.Local);

            return result;
        }
    }
}
