namespace ServiceAbonents.Dtos
{
    public class FilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string TariffId { get; set; } = string.Empty;
    }
}
