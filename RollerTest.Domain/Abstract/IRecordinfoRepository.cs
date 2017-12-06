using RollerTest.Domain.Entities;
using System.Linq;

namespace RollerTest.Domain.Abstract
{
    public interface IRecordinfoRepository
    {
        IQueryable<RollerRecordInfo> RollerRecordInfos { get; }

        void SaveRollerRecordInfo(RollerRecordInfo rollerrecordinfo);
        RollerRecordInfo DeleteRollerRecordInfo(int rollerrecordInfoID);
    }
}
