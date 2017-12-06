using RollerTest.Domain.Entities;
using System.Linq;

namespace RollerTest.Domain.Abstract
{
    public interface IBaseRepository
    {

        IQueryable<RollerBaseStation> RollerBaseStations { get; }
        IQueryable<TimerCfg> TimerCfgs { get; }

        IQueryable<ForcerCfg> ForcerCfgs { get; }


        void ChangeStationState(int StationId, bool state);
        void SaveForceInfo(string ForcerName, int ForceUp, int ForceDn, int ForceSet, bool ForceSwitch);

    }
}
