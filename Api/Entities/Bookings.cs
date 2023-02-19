using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class Bookings
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalPrice { get; set; }
        public bool Reservation { get; set; }

        public int? ApartamentId { get; set; }
        public virtual Apartaments? Apartament { get; set; }
        public int? ClientId { get; set; }
        public virtual Clients? Client { get; set; }

        public virtual List<Services> Services { get; set; } = new();
    }
}
