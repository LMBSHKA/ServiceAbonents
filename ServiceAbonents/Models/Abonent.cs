using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceAbonents.Models
{
    public class Abonent
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int TarrifId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PasportData { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public string DateForDeduct { get; set; }
    }
}