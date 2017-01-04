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

        // GET api/calendar/1/2016-12-05/2016-12-12
        [EnableCors("AllOrigins")]
        [HttpGet("{roomId}/{startTime}/{endTime}", Name = "GetBooking")]
        public async Task<JsonResult> Get(int roomId, DateTime startTime, DateTime endTime)
        {
            var result = await context.GetBookings(roomId, startTime, endTime);
            return Json(result);
        }

        // POST api/calendar/book
        [EnableCors("AllowHeaders")]
        [HttpPost("book")]
        public async Task<IActionResult> Create([FromBody]BookingCreate booking)
        {
            if (booking == null)
                return BadRequest();
            await context.BookRoom(booking);

            var routeParameters = new
            {
                roomId = booking.RoomId,
                startTime = booking.StartTime,
                endTime = booking.EndTime
            };

            var uri = "api/calendar/roomId/startTime/endTime";

            return Created(uri, routeParameters);
        }

        // PUT api/calendar/book/5
        [EnableCors("AllowHeaders")]
        [HttpPut("book/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody]BookingCreate newBooking)
        {
            if (newBooking == null || newBooking.Id != id)
                return BadRequest();

            var BookingToUpdate = context.FindExistingBooking(newBooking);

            if (BookingToUpdate == null)
                return NotFound();
            else if (BookingToUpdate.OccupantId != newBooking.OccupantId || BookingToUpdate.RoomId != newBooking.RoomId)
                return BadRequest();

            await context.UpdateBooking(newBooking);

            return new NoContentResult();
        }

        // DELETE api/calendar/book/5
        [EnableCors("AllowHeaders")]
        [HttpDelete("book/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookingToDelete = context.FindExistingBooking(id);
            if (bookingToDelete == null)
                return NotFound();

            await context.DeleteBooking(bookingToDelete);

            return new NoContentResult();
        }
    }
}
