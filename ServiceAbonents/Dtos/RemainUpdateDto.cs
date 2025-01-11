namespace ServiceAbonents.Dtos
{
    public class RemainUpdateDto
    {
        public short RemainGb { get; set; } = 0;
        public short RemainMin { get; set; } = 0;
        public short RemainSMS { get; set; } = 0;
        public bool? UnlimVideo { get; set; } = null;
        public bool? UnlimSocials { get; set; } = null;
        public bool? UnlimMusic { get; set; } = null;
        public bool? LongDistanceCall { get; set; } = null;
    }
}
