using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCalendarWebservice.Models.Entities
{
    public partial class CalendarContext
    {
        public async Task<RoomVM[]> GetAllRooms()
        {
            var rooms = await Room.ToArrayAsync();
            return Mapper.Map<RoomVM[]>(rooms);
        }

        public async Task<BookingVM[]> GetBookings(int roomId, DateTime start, DateTime end)
        {
            var bookings = await Booking.Where(r => r.Id == roomId && r.EndTime > start && r.StartTime < end).ToArrayAsync();
            return Mapper.Map<BookingVM[]>(bookings);
        }
    }
}
