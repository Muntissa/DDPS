using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDPS.Api.Entities
{
    public class Apartaments
    {
        public int Id { get; set; }

        [Display(Name = "Порядковый номер")]
        public int Number { get; set; }

        [Display(Name = "Новое фото")]
        public string? Photo { get; set; }

        [Display(Name = "Площадь")]
        public int Area { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Зарезервировано")]
        public bool Reservation { get; set; } = false;

        [Display(Name = "Тариф")]
        public int? TariffId { get; set; }
        public virtual Tariffs? Tariff { get; set; }
        


        public virtual List<Facilities> Facilities { get; set; } = new();
        public virtual List<Services> Services { get; set; } = new();
    }
}
