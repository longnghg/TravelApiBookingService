using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApi.Calendar;
using TravelApi.Helpers;

namespace TravelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleCalendarController : ControllerBase
    {
        [HttpPost]
        public async Task CreateGoogleCalendar(GoogleCalendar request)
        {
            await GoogleCalendarHelper.CreateGoogleCalendar(request);
        }
    }
}
