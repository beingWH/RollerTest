using RollerTest.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.Models.WTTESTMODEL.EFRepository
{
    public class EFTESTINFO : IReadRepository<WTTESTINFO>
    {
        OracleDbContext context = new OracleDbContext();
        public IEnumerable<WTTESTINFO> QueryEntities
        {
            get
            {
                return context.WTTestInfos;
            }
        }
    }
}