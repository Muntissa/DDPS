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

        [Display(Name = "Картинка")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Площадь")]
        public int Area { get; set; }

        [Display(Name = "Цена")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Price 
        { 
            get => (Tariff.Price * Services.Select(s => s.Price).Sum()) / Area; 
            private set { } 
        }

        [Display(Name = "Тариф"), Required]
        public int? TariffId { get; set; }
        public virtual Tariffs Tariff { get; set; }

        public virtual List<Facilities> Facilities { get; set; } = new();
        public virtual List<Services> Services { get; set; } = new();
    }
}
