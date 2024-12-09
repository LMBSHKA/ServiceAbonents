using ServiceAbonents.Dtos;

namespace ServiceAbonents.Data
{
    public interface IUpdateBalance
    {
        public bool TopUpBalance(TopUpDto newBalance);
    }
}
