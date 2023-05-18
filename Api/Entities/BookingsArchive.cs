using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class BookingsArchive
    {
        public int Id { get; set; }
        public int ApartamentNumber { get; set; }
        public string ClientSecondName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public virtual List<Services> ApartamentAdditionalServices { get; set; }
        
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }

        /*public virtual Bookings? Booking { get; set; }*/
        public DateTime InActiveTime { get; set; }
    }
}
