namespace ServiceAbonents.Dtos
{
    public class TarrifDto
    {
        public short RemainGb { get; set; }
        public short RemainMin { get; set; }
        public short RemainSMS { get; set; }
        public bool UnlimVideo { get; set; }
        public bool UnlimSocials { get; set; }
        public bool UnlimMusic { get; set; }
        public bool LongDistanceCall { get; set; }
    }
}
