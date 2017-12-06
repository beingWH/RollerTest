using RollerTest.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.Models.WTTESTMODEL.EFRepository
{
    public class EFSAMPLEINFO:IReadRepository<WTSAMPLEINFO>
    {
        OracleDbContext context = new OracleDbContext();

        public IEnumerable<WTSAMPLEINFO> QueryEntities
        {
            get
            {
                return context.WTSampleInfos;
            }
        }
    }
}