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
        public Tariffs Tariff { get; set; }
        public int Number { get; set; }
        public string? ImageUrl { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }
    }
}
