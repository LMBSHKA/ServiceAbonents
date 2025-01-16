using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class AbonentCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string Pasport { get; set; }
    }
}
