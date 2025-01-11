using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Models
{
    public class Remain
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid ClientId { get; set; }
        public short RemainGb { get; set; } = 0;
        public short RemainMin { get; set; } = 0;
        public short RemainSMS { get; set; } = 0;
        public bool UnlimVideo { get; set; } = false;
        public bool UnlimSocials { get; set; } = false;
        public bool UnlimMusic { get; set; } = false;
        public bool LongDistanceCall { get; set; } = false;
    }
}