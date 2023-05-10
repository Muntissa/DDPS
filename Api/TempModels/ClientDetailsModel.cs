using DDPS.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.TempModels
{
    public class ClientDetailsModel
    {
        public IEnumerable<Bookings> Bookings { get; set; }
        public Clients Client { get; set; }
    }
}
