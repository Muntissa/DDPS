using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DDPS.Api.Entities
{
    public class Clients
    {
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁё\s]*$", ErrorMessage = "Фамилия может состоять только из букв")]
        public string SecondName { get; set; }

        [Display(Name = "Имя")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁё\s]*$", ErrorMessage = "Имя может состоять только из букв")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁё\s]*$", ErrorMessage = "Отчество может состоять только из букв")]
        public string LastName { get; set; }

        [Display(Name = "Электронная почта")]
        [EmailAddress(ErrorMessage = "Некорректная электронная почта")]
        public string? Email { get; set; }

        [Display(Name = "Телефонный номер")]
        [Phone(ErrorMessage = "Некорректный телефонный номер")]
        public string? PhoneNumber { get; set; }
    }
}
