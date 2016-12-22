using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCalendarWebservice.Models
{
    public class BookingVM
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public int OccupantId { get; set; }
        public string Title { get; set; }
        public string Decription { get; set; }
        public string OccupantName { get; set; }
    }
}
