using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class SwitchTarifDto
    {
        [Required]
        public int Tarif { get; set; }
    }
}
