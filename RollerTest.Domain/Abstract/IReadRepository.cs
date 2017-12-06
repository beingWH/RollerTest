using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.Domain.Abstract
{
    public interface IReadRepository<T>
    {
        IEnumerable<T> QueryEntities { get; }
    }
}
