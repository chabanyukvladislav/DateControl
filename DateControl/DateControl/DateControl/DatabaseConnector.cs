using DateControl.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateControl
{
    class DatabaseConnector : IDatabaseConnector
    {
        private static DatabaseConnector _databaseConnector;
        private static readonly object Locker = new object();
        private readonly Context _context;

        public static DatabaseConnector GetDatabaseConnector
        {
            get
            {
                if (_databaseConnector == null)
                {
                    lock (Locker)
                    {
                        if (_databaseConnector == null)
                        {
                            _databaseConnector = new DatabaseConnector();
                        }
                    }
                }
                return _databaseConnector;
            }
        }

        private DatabaseConnector()
        {
            _context = new Context();
        }

        public async Task<bool> AddEvent(Event item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _context.Events.Add(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<bool> DeleteEvent(Event item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _context.Events.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<bool> EditEvent(Event newItem)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Event element = _context.Events.FirstOrDefault(el => el.Id == newItem.Id);
                    if (element == null)
                        return false;
                    element.Mounth = newItem.Mounth;
                    element.Day = newItem.Day;
                    element.Year = newItem.Year;
                    element.Heh = newItem.Heh;
                    element.Description = newItem.Description;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<List<Event>> GetEvents()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.Events.ToList();
                }
                catch (Exception)
                {
                    return new List<Event>();
                }
            });
        }

        public async Task<Event> GetEvent(int day, Mounths mounth, int year)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.Events.FirstOrDefault(el =>
                        el.Day == day && el.Mounth == mounth && el.Year == year);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
