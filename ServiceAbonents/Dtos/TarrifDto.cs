namespace ServiceAbonents.Dtos
{
    public class TarrifDto
    {
        public string TariffName { get; set; }
        public decimal RemainGb { get; set; }
        public decimal RemainMin { get; set; }
        public decimal RemainSMS { get; set; }
        public bool UnlimVideo { get; set; }
        public bool UnlimSocials { get; set; }
        public bool UnlimMusic { get; set; }
        public bool LongDistanceCall { get; set; }
    }
}
