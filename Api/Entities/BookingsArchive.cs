using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class BookingsArchive
    {
        public int Id { get; set; }
        public virtual Bookings? Booking { get; set; }
        public DateTime InActiveTime { get; set; }
    }
}
