namespace ServiceAbonents.Dtos
{
    public class RemainUpdateDto
    {
        public decimal? RemainGb { get; set; } = null;
        public decimal? RemainMin { get; set; } = null;
        public decimal? RemainSMS { get; set; } = null;
        public bool? UnlimVideo { get; set; } = null;
        public bool? UnlimSocials { get; set; } = null;
        public bool? UnlimMusic { get; set; } = null;
        public bool? LongDistanceCall { get; set; } = null;
    }
}
