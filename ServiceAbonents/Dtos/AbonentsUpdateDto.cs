﻿namespace ServiceAbonents.Dtos
{
    public class AbonentsUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string PasportData { get; set; } = string.Empty;
    }
}
