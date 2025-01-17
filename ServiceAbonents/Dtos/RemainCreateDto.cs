using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class RemainCreateDto
    {
        [Required]
        public Guid ClientId { get; set; }
        public decimal RemainGb { get; set; } = 0;
        public decimal RemainMin { get; set; } = 0;
        public decimal RemainSMS { get; set; } = 0;
        public bool UnlimVideo { get; set; } = false;
        public bool UnlimSocials { get; set; } = false;
        public bool UnlimMusic { get; set; } = false;
        public bool LongDistanceCall { get; set; } = false;
    }
}
