using System;
using System.Collections.Generic;

namespace AcademyCalendarWebservice.Models.Entities
{
    public partial class Room
    {
        public Room()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool? HasWhiteBoard { get; set; }
        public bool? HasProjector { get; set; }
        public bool? HasTvScreen { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
