using genvcaredashboardsAPI.Entities;
using genvcaredashboardsAPI.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace genvcaredashboardsAPI.Services
{
    public interface IEventService
    {
       // Task<IEnumerable<EventType>> GetEventTypesAsync();

        Task<List<AllUniqueEventsAndTestsDTO>> GetAllUniqueEventsAndTestsAsync();
        Task<List<AllUniqueAbnormalEventsAndTestsDTO>> GetAllUniqueAbnormalEventsAndTestsAsync();
    }
}
