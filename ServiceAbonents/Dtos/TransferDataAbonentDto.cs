using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class TransferDataAbonentDto
    {
        [Required]
        public Guid AbonentId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string TarifId { get; set; }
        [Required]
        public decimal TarifCost { get; set; }
    }
}
