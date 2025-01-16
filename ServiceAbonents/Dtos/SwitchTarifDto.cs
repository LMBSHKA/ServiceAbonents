using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class SwitchTarifDto
    {
        public short? Minutes { get; set; }
        public short? Gigabytes { get; set; }
        public short? Sms { get; set; }
        public bool? UnlimVideo { get; set; }
        public bool? UnlimSocials { get; set; }
        public bool? UnlimMusic { get; set; }
        public bool? LongDistanceCall { get; set; }
    }
}