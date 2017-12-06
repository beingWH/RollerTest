using RollerTest.Domain.Abstract;
using System.Linq;
using RollerTest.Domain.Entities;

namespace RollerTest.Domain.Concrete
{
    public class EFTestreportinfoRepository : ITestreportinfoRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<RollerTestreportInfo> RollerTestreportInfos
        {
            get
            {
                return context.RollerTestreportInfos;
            }
        }
        public void SaveRollerTestreportInfo(RollerTestreportInfo rollertestreportinfo)
        {
            if (rollertestreportinfo.RollerTestReportInfoID == 0)
            {
                context.RollerTestreportInfos.Add(rollertestreportinfo);
            }
            else
            {
                RollerTestreportInfo dbEntry = context.RollerTestreportInfos.Find(rollertestreportinfo.RollerTestReportInfoID);
                if (dbEntry != null)
                {
                    dbEntry.RollerTestReportInfoID = rollertestreportinfo.RollerTestReportInfoID;
                    dbEntry.RollerSampleInfoID = rollertestreportinfo.RollerSampleInfoID;
                    dbEntry.StartStatus = rollertestreportinfo.StartStatus;
                    dbEntry.StartText = rollertestreportinfo.StartText;
                    dbEntry.EndText = rollertestreportinfo.EndText;
                    dbEntry.EndStatus = rollertestreportinfo.EndStatus;
                    dbEntry.FinalStatus = rollertestreportinfo.FinalStatus;
                }
            }
            context.SaveChanges();
        }
    }
}
