using System;
using System.Collections.Generic;

namespace AcademyCalendarWebservice.Models.Entities
{
    public partial class Booking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public int OccupantId { get; set; }
        public string Title { get; set; }
        public string Decription { get; set; }

        public virtual Occupant Occupant { get; set; }
        public virtual Room Room { get; set; }
    }
}
