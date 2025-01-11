using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class SwitchTarifDto
    {
        [Required]
        public string Tarif { get; set; }
    }
}
