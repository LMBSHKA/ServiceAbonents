namespace ServiceAbonents.Dtos
{
    public class RemainUpdateDto
    {
        public short? RemainGb { get; set; } = null;
        public short? RemainMin { get; set; } = null;
        public short? RemainSMS { get; set; } = null;
        public bool? UnlimVideo { get; set; } = null;
        public bool? UnlimSocials { get; set; } = null;
        public bool? UnlimMusic { get; set; } = null;
        public bool? LongDistanceCall { get; set; } = null;
    }
}
