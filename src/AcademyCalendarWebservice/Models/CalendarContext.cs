using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var bookingVM = Mapper.Map<BookingVM[]>(bookings);

            foreach (var booking in bookingVM)
            {
                var occupant = Occupant.FirstOrDefault(o => o.Id == booking.OccupantId);
                booking.OccupantName = occupant.FirstName + " " + occupant.LastName;
            }

            return bookingVM;
        }
        public async Task BookRoom(BookingCreate booking)
        {
            var bookingToDb = Mapper.Map<Booking>(booking);
            Booking.Add(bookingToDb);
            await SaveChangesAsync();
        }

        public Booking FindExistingBooking(BookingCreate newBooking)
        {
            // It's okay to return null, that's handled in the Controller.
            return Booking.First(b => b.Id == newBooking.Id);
        }

        public Booking FindExistingBooking(int bookingId)
        {
            return Booking.First(b => b.Id == bookingId);
        }

        public async Task UpdateBooking(BookingCreate booking)
        {
            var bookingToUpdate = FindExistingBooking(booking);
            var newBooking = Mapper.Map<Booking>(booking);

            // Using reflection to set all properties of the existing booking to the ones coming in.
            foreach (PropertyInfo propertyInfo in bookingToUpdate.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                    propertyInfo.SetValue(bookingToUpdate, propertyInfo.GetValue(newBooking));
            }

            await SaveChangesAsync();
        }

        public async Task DeleteBooking(Booking booking)
        {
            Booking.Remove(booking);
            await SaveChangesAsync();
        }
    }
}
