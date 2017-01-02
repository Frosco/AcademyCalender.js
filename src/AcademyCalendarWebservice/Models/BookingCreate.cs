using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCalendarWebservice.Models
{
    // An incoming new booking to the Web API.
    public class BookingCreate
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public int OccupantId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
