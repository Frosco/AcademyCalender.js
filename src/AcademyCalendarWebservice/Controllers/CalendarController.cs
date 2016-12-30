using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AcademyCalendarWebservice.Models.Entities;
using AcademyCalendarWebservice.Models;
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("AllOrigins")]
        [HttpGet("rooms")]
        public async Task<JsonResult> Get()
        {
            var result = await context.GetAllRooms();
            return Json(result);
        }

        // GET api/calendar/roomId/startTime/endTime
        [EnableCors("AllOrigins")]
        [HttpGet("{roomId}/{startTime}/{endTime}")]
        public async Task<JsonResult> Get(int roomId, DateTime startTime, DateTime endTime)
        {
            var result = await context.GetBookings(roomId, startTime, endTime);
            return Json(result);
        }

        // POST api/calendar/book
        [EnableCors("AllowHeaders")]
        [HttpPost("book")]
        public async Task<bool> Create([FromBody]BookingVM booking)
        {
            if (booking == null)
                return false;
            //await context.BookRoom(booking);
            var message = booking;
            return true;
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
