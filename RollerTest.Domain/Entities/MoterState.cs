using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.Domain.Entities
{
    /// <summary>
    /// 电机状态模型
    /// </summary>
    public class MoterState
    {
        public bool IsMotoRunning { get; set; }
        public int MotoREV { get; set; }
        public int MotoSPD { get; set; }
        public bool ErrMotoDis { get; set; }
    }
}
