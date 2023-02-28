using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DDPS.Api.Entities
{
    public class Bookings
    {
        public int Id { get; set; }

        [Display(Name = "Начальная дата")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Конечная дата")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Общая цена")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int TotalPrice 
        {
            get => (Apartament.Price + Services.Select(s => s.Price).Sum()) * (int)(EndTime - StartTime).TotalDays;
            private set { }
        }

        [Display(Name = "Бронирование")]
        public bool Reservation { get; set; }

        [Display(Name = "Апартаменты")]
        public int? ApartamentId { get; set; }
        public virtual Apartaments? Apartament { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        public virtual Clients? Client { get; set; }
        public virtual List<Services> Services { get; set; } = new();
    }
}
