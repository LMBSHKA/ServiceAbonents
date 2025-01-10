using ServiceAbonents.Dtos;

namespace ServiceAbonents.Data
{
    public interface ISwitchTarif
    {
        void UpdateTarif(SwitchTarifDto newTarif, int AbonentId);
    }
}
