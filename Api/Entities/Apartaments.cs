using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class Apartaments
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? ImageUrl { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }

        public int? TariffId { get; set; }
        public virtual Tariffs? Tariff { get; set; }

        public virtual List<Facilities> Facilities { get; set; } = new();
        public virtual List<Services> Services { get; set; } = new();
    }
}
