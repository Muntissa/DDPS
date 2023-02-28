using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DDPS.Api.Entities
{
    public class Services
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        public virtual List<Apartaments> Apartaments { get; set; } = new();
        public virtual List<Bookings> Bookings { get; set; } = new();
    }
}
