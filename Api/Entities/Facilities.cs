using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DDPS.Api.Entities
{
    public class Facilities
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        public virtual List<Apartaments>? Apartaments { get; set; } = new();
    }
}
