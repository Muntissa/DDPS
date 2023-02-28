using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DDPS.Api.Entities
{
    public class Clients
    {
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string LastName { get; set; }

        [Display(Name = "Электронная почта")]
        public string? Email { get; set; }

        [Display(Name = "Телефонный номер")]
        public string? PhoneNumber { get; set; }
    }
}
