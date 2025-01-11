using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class RemainReadDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public short RemainGb { get; set; }
        public short RemainMin { get; set; }
        public short RemainSMS { get; set; }
        public bool UnlimVideo { get; set; }
        public bool UnlimSocials { get; set; }
        public bool UnlimMusic { get; set; }
        public bool LongDistanceCall { get; set; }
    }
}
