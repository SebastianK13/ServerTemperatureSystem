using System;

namespace ServerTemperatureSystem.Services
{
    public static class TimeService
    {
        public static DateTime Last20Minutes()
        {
            DateTime date = DateTime.Now;
            date = date.AddMinutes(-20);
            date = date.AddSeconds(-30);
            date = date.AddMilliseconds(-date.Millisecond);

            return date;
        }
        public static DateTime RoundToMinutes(int additionalMinutes = 0)
        {
            DateTime date = DateTime.Now;
            date = date.AddSeconds(-date.Second);
            date = date.AddMilliseconds(-date.Millisecond);
            date = date.AddMinutes(additionalMinutes);

            return date;
        }
    }
}