namespace ServiceAbonents.Dtos
{
    public class TransferForAuthDto
    {
        public Guid AbonentId { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
