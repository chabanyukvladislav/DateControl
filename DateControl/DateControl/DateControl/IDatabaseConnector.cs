using DateControl.Calendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DateControl
{
    interface IDatabaseConnector
    {
        Task<List<Event>> GetEvents();
        Task<Event> GetEvent(int day, Mounths mounth, int year);
        Task<bool> AddEvent(Event item);
        Task<bool> EditEvent(Event newItem);
        Task<bool> DeleteEvent(Event item);
    }
}
