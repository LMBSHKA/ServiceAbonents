using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class AbonentReadDto 
    {
        public Guid Id { get; set; }
        public string TariffId { get; set; }
        public string TariffName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string Pasport { get; set; }
        public decimal Balance { get; set; }
        public string DateForDeduct { get; set; }
        public bool Status { get; set; }
        public decimal TariffCost { get; set; }
    }
}
