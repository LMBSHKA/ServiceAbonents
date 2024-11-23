namespace ServiceAbonents.Dtos
{
    public class AbonentsUpdateDto
    {
        public int TarrifId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasportData { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
    }
}
