using System;
using System.Collections.Generic;

namespace AcademyCalendarWebservice.Models.Entities
{
    public partial class Occupant
    {
        public Occupant()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
