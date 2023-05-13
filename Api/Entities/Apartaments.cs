﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDPS.Api.Entities
{
    public class Apartaments
    {
/*        [NotMapped]
        private int _totalPrice
        {
            get
            {
                try
                {
                    return this.Tariff.Price + (this.Area * 50) + (int)(0.5 * this.Services.Sum(s => s.Price));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }*/

        public int Id { get; set; }

        [Display(Name = "Порядковый номер")]
        public int Number { get; set; }

        [Display(Name = "Картинка")]
        public string? Photo { get; set; }

        [Display(Name = "Площадь")]
        public int Area { get; set; }

/*        [Display(Name = "Цена")]
        public int Price { get; set; }
*/
        [Display(Name = "Зарезервировано")]
        public bool Reservation { get; set; } = false;

        [Display(Name = "Тариф")]
        public int? TariffId { get; set; }
        public virtual Tariffs? Tariff { get; set; }
        
        public virtual List<Facilities>? Facilities { get; set; } = new();
        public virtual List<Services>? Services { get; set; } = new();
    }
}
