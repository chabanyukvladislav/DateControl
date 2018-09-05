using System;
using System.Collections.Generic;
using System.Linq;
using DateControl.Calendar;

namespace DateControl
{
    class EventsCollection : IEventsCollection
    {
        private static EventsCollection _eventCollection;
        private static readonly object Locker = new object();
        private readonly IDatabaseConnector _databaseConnector;

        public static EventsCollection GetEventsCollection
        {
            get
            {
                if (_eventCollection == null)
                {
                    lock (Locker)
                    {
                        if (_eventCollection == null)
                        {
                            _eventCollection = new EventsCollection();
                        }
                    }
                }
                return _eventCollection;
            }
        }

        private List<Event> Collection { get; set; } = new List<Event>();

        private EventsCollection()
        {
            _databaseConnector = DatabaseConnector.GetDatabaseConnector;
            LoadCollection();
        }

        private async void LoadCollection()
        {
            Collection = await _databaseConnector.GetEvents() ?? new List<Event>();
            CollectionChanged?.Invoke();
        }

        public async void AddEvent(Event item)
        {
            if (await _databaseConnector.AddEvent(item) == false)
                return;
            Event element = await _databaseConnector.GetEvent(item.Day, item.Mounth, item.Year);
            if (element == null)
                return;
            Collection.Add(element);
            CollectionChanged?.Invoke();
        }

        public async void DeleteEvent(Event item)
        {
            if (await _databaseConnector.DeleteEvent(item) == false)
                return;
            Collection.Remove(item);
            CollectionChanged?.Invoke();
        }

        public async void EditEvent(Event newItem)
        {
            if (await _databaseConnector.EditEvent(newItem) == false)
                return;
            Collection.Remove(Collection.Find(el => el.Id == newItem.Id));
            Collection.Add(newItem);
            CollectionChanged?.Invoke();
        }

        public Event GetEvent(int day, Mounths mounth, int year)
        {
            return Collection.FirstOrDefault(el => el.Day == day && el.Mounth == mounth && el.Year == year);
        }

        public event Action CollectionChanged;
    }
}
