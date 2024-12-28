using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace ServiceAbonents.Models
{
    public class User : BaseEntity
    {
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
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PasportData { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public bool Status { get; set; } = false;
        public List<Role> Roles { get; set; } = new();
        public string DateForDeduct { get; set; }
    }
}