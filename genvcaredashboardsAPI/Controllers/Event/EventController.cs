using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using genvcaredashboardsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using genvcaredashboardsAPI.Helpers;

namespace genvcaredashboardsAPI.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;
        private readonly IConfiguration _configuration;

        public EventController(ILogger<EventController> logger, IConfiguration configuration,IEventService eventService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._eventService = eventService;
           
        }
        ///// <summary>
        ///// Get all the Event types in the system
        ///// </summary>
        ///// <returns>An ActionResult of type IEnumerable of EventTypeDTO</returns>
        //[HttpGet(Name = "GetEventTypes")]
        //public async Task<IActionResult> GetEventTypes()
        //{
        //    //Read rercords form repository
        //    var eventFromRepo = await _eventService.GetEventTypesAsync();

        //    //If retturned result is null then send 404
        //    if (eventFromRepo == null)
        //    {
        //        return NotFound();
        //    }

        //    //return Ok(_mapper.Map<IEnumerable<EventTypeDto>>(eventFromRepo));

        //    return Ok(eventFromRepo);
        //}


        /// <summary>
        /// Get Fetch All Unique Events And Tests in the system
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of FetchAllUniqueEventsAndTestsDTO</returns>
        [HttpGet("GetAllUniqueEventsAndTests", Name = "GetAllUniqueEventsAndTests")]
        public async Task<IActionResult> GetAllUniqueEventsAndTests()
        { 
            try
            {
                //Read rercords form repository
                var eventFromRepo = await _eventService.GetAllUniqueEventsAndTestsAsync();

                //If retturned result is null then send 404
                if (eventFromRepo == null)
                {
                    return NotFound();
                }

                return Ok(eventFromRepo);

                //return Ok(_mapper.Map<AccountDto>(accountRepo));
            }
            catch (Exception e)
            {
                _logger.LogError("API: GetFetchAllUniqueEventsAndTests =>" + e.Message + e.StackTrace);
                return Problem(APIHelper.GenerateErrorMessage(_configuration["CustomErrorMessage"], e));
            }

        }

        /// Get FETCH ALL Unique ABNORMAL Events & Tests in the system
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of UniqueAbnormalEventsAndTestsDTO</returns>
        [HttpGet("GetAllUniqueAbnormalEventsAndTests", Name = "GetAllUniqueAbnormalEventsAndTests")]
        public async Task<IActionResult> GetAllUniqueAbnormalEventsAndTests()
        {
            try
            {
                //Read rercords form repository
                var eventFromRepo = await _eventService.GetAllUniqueAbnormalEventsAndTestsAsync();

                //If retturned result is null then send 404
                if (eventFromRepo == null)
                {
                    return NotFound();
                }

                return Ok(eventFromRepo);

                //return Ok(_mapper.Map<AccountDto>(accountRepo));
            }
            catch (Exception e)
            {
                _logger.LogError("API: GetAllUniqueAbnormalEventsAndTests =>" + e.Message + e.StackTrace);
                return Problem(APIHelper.GenerateErrorMessage(_configuration["CustomErrorMessage"], e));
            }

        }
    }
}
