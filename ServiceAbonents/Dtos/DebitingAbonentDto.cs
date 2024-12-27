using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class DebitingAbonentDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TarrifId { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}
