using RollerTest.Domain.Abstract;
using System;
using System.Linq;
using RollerTest.Domain.Entities;

namespace RollerTest.Domain.Concrete
{
    public class EFSampleinfoRepository : ISampleinfoRepository
    {
        private EFDbContext context = new EFDbContext();


        public IQueryable<RollerSampleInfo> RollerSampleInfos
        {
            get
            {
                return context.RollerSampleInfos;
            }
            
        }

        public RollerSampleInfo DeleteRollerSampleInfo(int rollersampleinfoID)
        {
            using (EFDbContext context = new EFDbContext())
            {
                RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(rollersampleinfoID);
                if (dbEntry != null)
                {
                    context.RollerSampleInfos.Remove(dbEntry);
                    context.SaveChanges();
                }
                return dbEntry;
            }
        }

        public void SaveRollerSampleInfo(RollerSampleInfo rollersampleinfo)
        {
            using (EFDbContext context = new EFDbContext())
            {
                if (rollersampleinfo.RollerSampleInfoID == 0)
                {
                    context.RollerSampleInfos.Add(rollersampleinfo);
                }
                else
                {
                    RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(rollersampleinfo.RollerSampleInfoID);
                    if (dbEntry != null)
                    {
                        dbEntry.RollerProjectInfoID = rollersampleinfo.RollerProjectInfoID;
                        dbEntry.SampleID = rollersampleinfo.SampleID;
                        dbEntry.SampleName = rollersampleinfo.SampleName;
                        dbEntry.SetValue = rollersampleinfo.SetValue;
                        dbEntry.RollerBaseStationID = rollersampleinfo.RollerBaseStationID;
                        dbEntry.UpLimit = rollersampleinfo.UpLimit;
                        dbEntry.DnLimit = rollersampleinfo.DnLimit;
                        dbEntry.TestType = rollersampleinfo.TestType;
                        dbEntry.TestTime = rollersampleinfo.TestTime;
                        dbEntry.State = rollersampleinfo.State;
                        dbEntry.TestID = rollersampleinfo.TestID;
                    }
                }
                context.SaveChanges();
            }
        }
        public void setsampleState(int Id,string state)
        {
            using(EFDbContext context=new EFDbContext())
            {
                RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.State = state;
                }
                context.SaveChangesAsync().Wait();
            }

        }
        public void setsampleStartTime(RollerSampleInfo rollersampleinfo)
        {
            using (EFDbContext context = new EFDbContext())
            {
                RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(rollersampleinfo.RollerSampleInfoID);
                if (dbEntry != null && dbEntry.StartTime == null)
                {
                    dbEntry.StartTime = DateTime.Now;
                }
                context.SaveChangesAsync().Wait();
            }

        }
        public void setsampleEndTime(int Id)
        {
            using (EFDbContext context = new EFDbContext())
            {
                RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.EndTime = DateTime.Now;
                }
                context.SaveChangesAsync().Wait();
            }

        }
        public void setsampleTotalTime(int Id,TimeSpan time)
        {
            using (EFDbContext context = new EFDbContext())
            {
                RollerSampleInfo dbEntry = context.RollerSampleInfos.Find(Id);
                if (dbEntry != null)
                {
                    dbEntry.TestTotalTime = time.ToString();
                }
                context.SaveChangesAsync().Wait();
            }
        }
    }
}
