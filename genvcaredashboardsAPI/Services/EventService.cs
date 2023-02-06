using genvcaredashboardsAPI.DBContexts;
using genvcaredashboardsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using genvcaredashboardsAPI.Models.Event;

namespace genvcaredashboardsAPI.Services
{
    public class EventService : IEventService, IDisposable
    {
        private GenVCareContext _context;
       // private readonly IConfiguration _configuration;
       // private readonly IMapper _mapper;
       // private readonly IPropertyMappingService _propertyMappingService;

        public EventService(GenVCareContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            //this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            //this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //_propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        #region Event Type
        //public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        //{
        //    return await _context.EventType.ToListAsync();
        //}


        public async Task<List<AllUniqueEventsAndTestsDTO>> GetAllUniqueEventsAndTestsAsync()
        { 
            try
            {
                var adata = await _context.GetFetchAllUniqueEventsAndTests.FromSqlRaw("EXEC FetchAllUniqueEventsAndTests").ToListAsync();
                return adata;
            }
            catch(Exception ex)
            {
                throw ex;
            } 
        }

        public async Task<List<AllUniqueAbnormalEventsAndTestsDTO>> GetAllUniqueAbnormalEventsAndTestsAsync()
        {
            try
            {
                var adata = await _context.GetFetchAllUniqueAbnormalEventsAndTests.FromSqlRaw("EXEC FetchAllUniqueAbnormalEventsAndTests").ToListAsync();
                return adata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Dispose 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose resources when needed
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
        #endregion
    }
}
