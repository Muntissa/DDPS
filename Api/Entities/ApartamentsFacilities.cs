using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class ApartamentsFacilities
    {
        public int Id { get; set; }
        public int ApartamentId { get; set;}
        public int FacilityId { get; set; }

        public virtual Apartaments Apartament { get; set; }
        public virtual Facilities Facility { get; set; } 
    }
}
