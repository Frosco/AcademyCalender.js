using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AcademyCalendarWebservice.Models.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AcademyCalendarWebservice.Controllers
{
    [Route("api/[controller]")]
    public class CalendarController : Controller
    {
        CalendarContext context;

        public CalendarController(CalendarContext context)
        {
            this.context = context;
        }

        // GET: api/calendar/rooms
        [HttpGet("rooms")]
        public async Task<JsonResult> Get()
        {
            var result = await context.GetAllRooms();
            return Json(result);
        }

        // GET api/calendar/id/startTime/endTime
        [HttpGet("{id}/{startTime}/{endTime}")]
        public async Task<JsonResult> Get(int id, DateTime startTime, DateTime endTime)
        {
            var result = await context.GetBookings(id, startTime, endTime);
            return Json(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
