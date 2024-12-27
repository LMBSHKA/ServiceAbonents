using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class AbonentCreateDto
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
        public string PasportData { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public string DateForDeduct { get; set; }
    }
}
