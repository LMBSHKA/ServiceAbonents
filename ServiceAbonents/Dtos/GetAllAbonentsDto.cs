namespace ServiceAbonents.Dtos
{
    public class GetAllAbonentsDto
    {
        public IEnumerable<AbonentReadDto> abonentRead { get; set; }
        public double PageCount { get; set; }
    }
}
