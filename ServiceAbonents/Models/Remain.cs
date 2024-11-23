using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Models
{
    public class Remain
    {
        [Key]
        [Required]
        public int Id { get; set; } 
        [Required]
        public int ClientId { get; set; }
        public short ReaminGb { get; set; }
        public short RemainMin { get; set; }
        public short RemainSMS { get; set; }
    }
}
