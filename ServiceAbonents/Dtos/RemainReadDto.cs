using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class RemainReadDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public short RemainGb { get; set; }
        public short RemainMin { get; set; }
        public short RemainSMS { get; set; }
    }
}
