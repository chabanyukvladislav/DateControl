using System;

namespace DateControl.Calendar
{
    public class Event
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public Mounths Mounth { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Heh { get; set; }
    }
}
