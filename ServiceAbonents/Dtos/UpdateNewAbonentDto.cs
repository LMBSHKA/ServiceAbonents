using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class UpdateNewAbonentDto
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string TariffId { get; set; }
        [Required]
        public decimal TariffCost { get; set; }
    }
}