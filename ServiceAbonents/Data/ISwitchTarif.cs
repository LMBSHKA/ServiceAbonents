using ServiceAbonents.Dtos;

namespace ServiceAbonents.Data
{
    public interface ISwitchTarif
    {
        void UpdateTarif(SwitchTarifDto newTariff, Guid AbonentId, string tariffId, decimal tariffCost);
    }
}
