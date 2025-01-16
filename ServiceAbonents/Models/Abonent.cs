using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceAbonents.Models
{
    public class Abonent
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string TariffId { get; set; } = string.Empty;
        public string TariffName { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Pasport { get; set; }
        [Required]
        public decimal Balance { get; set; } = 0;
        public string DateForDeduct { get; set; } = string.Empty;
        [Required]
        public bool Status { get; set; } = false;
        [Required]
        public decimal TariffCost { get; set; } = 0;
        [Required]
        public string Role { get; set; } = "User";
    }
}