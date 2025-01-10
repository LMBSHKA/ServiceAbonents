using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class TransferDataAbonentDto
    {
        [Required]
        public int AbonentId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int TarifId { get; set; }
        [Required]
        public decimal TarifCost { get; set; }
    }
}
