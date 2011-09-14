using System;

namespace TimeTrackerCQRS.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime GetUtcNow();
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}