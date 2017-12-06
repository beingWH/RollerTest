using RollerTest.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.Models.WTTESTMODEL.EFRepository
{
    public class EFTESTEQUIPMENT : IReadRepository<WTTESTEQUIPMENT>
    {
        OracleDbContext context = new OracleDbContext();
        public IEnumerable<WTTESTEQUIPMENT> QueryEntities
        {
            get
            {
                return context.WTTestEquipments;
            }
        }
    }
}