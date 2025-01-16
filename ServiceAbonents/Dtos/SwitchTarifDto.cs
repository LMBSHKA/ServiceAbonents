using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class SwitchTarifDto
    {
        public decimal? Minutes { get; set; }
        public decimal? Gigabytes { get; set; }
        public decimal? Sms { get; set; }
        public bool? UnlimVideo { get; set; }
        public bool? UnlimSocials { get; set; }
        public bool? UnlimMusic { get; set; }
        public bool? LongDistanceCall { get; set; }
    }
}