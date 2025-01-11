using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class TopUpDto
    {
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public decimal Amount { get ; set; }

    }
}
