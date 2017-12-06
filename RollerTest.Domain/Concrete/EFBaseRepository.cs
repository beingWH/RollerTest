using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using System.Linq;

namespace RollerTest.Domain.Concrete
{
    public class EFBaseRepository:IBaseRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<RollerBaseStation> RollerBaseStations
        {
            get
            {
                return context.RollerBaseStations;
            }
        }
        public IQueryable<TimerCfg> TimerCfgs
        {
            get
            {
                return context.TimerCfgs;
            }
        }
        public IQueryable<ForcerCfg> ForcerCfgs
        {
            get
            {
                return context.ForcerCfgs;
            }
        }
        public void SaveForceSwitch(string ForceName, bool ForceSwtich)
        {
            using (EFDbContext context = new EFDbContext()) {
                int Id = context.ForcerCfgs.FirstOrDefault(x => x.ForcerName == ForceName).Id;
                ForcerCfg dbEntry = context.ForcerCfgs.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.ForcerSwitch = ForceSwtich;
                }
                context.SaveChangesAsync().Wait();
            }

        }
        public void SaveTimeSwitch(string TimerName, bool TimeSwitch)
        {
            using (EFDbContext context = new EFDbContext()) {
                int Id = context.TimerCfgs.FirstOrDefault(x => x.TimerName == TimerName).Id;
                TimerCfg dbEntry = context.TimerCfgs.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.TimerSwitch = TimeSwitch;
                }
                context.SaveChangesAsync().Wait();
            }       
        }
        public void ChangeStationState(int StationId,bool state)
        {
            using (EFDbContext context = new EFDbContext()) {
                RollerBaseStation dbEntry = context.RollerBaseStations.Find(StationId);
                if (dbEntry != null)
                {
                    dbEntry.State = state;
                }
                context.SaveChangesAsync().Wait();
            }
        }
        public void SaveForceInfo(string ForcerName, int ForceUp,int ForceDn,int ForceSet,bool ForceSwitch)
        {
            using (EFDbContext context = new EFDbContext()) {
                int Id = context.ForcerCfgs.FirstOrDefault(x => x.ForcerName == ForcerName).Id;
                ForcerCfg dbEntry = context.ForcerCfgs.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.ForcerUp = ForceUp;
                    dbEntry.ForcerDn = ForceDn;
                    dbEntry.ForcerSet = ForceSet;
                    dbEntry.ForcerSwitch = ForceSwitch;
                }
                context.SaveChangesAsync().Wait();
            }

        }

    }
}
