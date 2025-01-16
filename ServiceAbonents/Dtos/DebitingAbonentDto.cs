using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class DebitingAbonentDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public decimal TariffCost { get; set; }
    }
}
