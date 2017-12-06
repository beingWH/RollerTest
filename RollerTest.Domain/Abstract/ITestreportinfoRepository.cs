using RollerTest.Domain.Entities;
using System.Linq;

namespace RollerTest.Domain.Abstract
{
    public interface ITestreportinfoRepository
    {
        IQueryable<RollerTestreportInfo> RollerTestreportInfos { get; }
        void SaveRollerTestreportInfo(RollerTestreportInfo rollertestreportinfo);
        //RollerTestreportInfo DeleteRollerTestreportInfo(int rollertestreportinfoID);

    }
}
