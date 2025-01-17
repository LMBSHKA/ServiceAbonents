using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class RemainReadDto
    {
        public decimal RemainGb { get; set; }
        public decimal RemainMin { get; set; }
        public decimal RemainSMS { get; set; }
    }
}
