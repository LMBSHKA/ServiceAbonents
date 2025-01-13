namespace ServiceAbonents.Dtos
{
    public class AbonentsUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Pasport { get; set; } = string.Empty;
        public int TariffCost { get; set; } = 0;
        public bool? Status { get; set; } = null;
    }
}
