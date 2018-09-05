using System;
using DateControl.Calendar;

namespace DateControl
{
    interface IEventsCollection
    {
        Event GetEvent(int day, Mounths mounth, int year);
        void AddEvent(Event item);
        void DeleteEvent(Event item);
        void EditEvent(Event newItem);
        event Action CollectionChanged;
    }
}
