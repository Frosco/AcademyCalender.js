using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCalendarWebservice.Models
{
    public class RoomVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasWhiteBoard { get; set; }
        public bool HasProjector { get; set; }
        public bool HasTvScreen { get; set; }
    }
}
