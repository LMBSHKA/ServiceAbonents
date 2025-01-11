using ServiceAbonents.Dtos;

namespace ServiceAbonents.Data
{
    public interface ISwitchTarif
    {
        void UpdateTarif(SwitchTarifDto newTarif, Guid AbonentId);
    }
}
