using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class Facilities
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Apartaments> Apartaments { get; set; } = new();
    }
}
