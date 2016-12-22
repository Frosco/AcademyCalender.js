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
            var bookings = await Booking.Where(r => r.RoomId == roomId && r.EndTime > start && r.StartTime < end).ToArrayAsync();
            var bookingVM =  Mapper.Map<BookingVM[]>(bookings);

            foreach (var booking in bookingVM)
            {
                var occupant = Occupant.FirstOrDefault(o => o.Id == booking.OccupantId);
                booking.OccupantName = occupant.FirstName + " " + occupant.LastName;
            }

            return bookingVM;
        }
        public async Task BookRoom(BookingVM booking)
        {
            var newBooking = Mapper.Map<Booking>(booking);

            Booking.Add(newBooking);
            await SaveChangesAsync();
        }
    }
}
