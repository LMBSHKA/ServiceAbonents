using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Models
{
    public class Remain : BaseEntity
    {
        [Required]
        public Guid ClientId { get; set; }
        public short ReaminGb { get; set; }
        public short RemainMin { get; set; }
        public short RemainSMS { get; set; }
    }
}
