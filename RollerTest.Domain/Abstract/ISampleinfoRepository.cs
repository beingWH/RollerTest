using RollerTest.Domain.Entities;
using System;
using System.Linq;

namespace RollerTest.Domain.Abstract
{
    public interface ISampleinfoRepository
    {
        IQueryable<RollerSampleInfo> RollerSampleInfos { get; }
        void SaveRollerSampleInfo(RollerSampleInfo rollersampleinfo);

        RollerSampleInfo DeleteRollerSampleInfo(int rollersampleinfoID);
        void setsampleState(int Id, string state);
        void setsampleStartTime(RollerSampleInfo rollersampleinfo);
        void setsampleEndTime(int Id);
        void setsampleTotalTime(int Id, TimeSpan time);

    }
}
