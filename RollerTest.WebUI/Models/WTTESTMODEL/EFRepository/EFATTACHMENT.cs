using RollerTest.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.Models.WTTESTMODEL.EFRepository
{
    public class EFATTACHMENT : IReadRepository<WTATTACHMENT>
    {
        OracleDbContext context = new OracleDbContext();
        public IEnumerable<WTATTACHMENT> QueryEntities
        {
            get
            {
                return context.WTAttachments;
            }
        }
    }
}