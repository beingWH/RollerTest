using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RollerTest.Domain.Entities
{
    public class RollerBaseStation
    {
        [Key]
        public int RollerBaseStationID { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string Device { get; set; }
        [Column(TypeName = "varchar2"),MaxLength(100)]
        public string Station { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string Mark { get; set; }
        public bool State { get; set; }
        [ForeignKey("TimerCfg")]
        public int TimerCfgId { get; set; }
        public virtual TimerCfg TimerCfg { get; set; }
        [ForeignKey("ForcerCfg")]
        public int ForcerCfgId { get; set; }
        public virtual ForcerCfg ForcerCfg { get; set; }
        public virtual ICollection<RollerSampleInfo> RollerSampleInfo { get; set; }

    }
    public class TimerCfg
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string TimerName { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string TimerMark { get; set; }
        public bool TimerSwitch { get; set; }
        public virtual ICollection<RollerBaseStation> RollerBaseStation { get; set; }

    }
    public class ForcerCfg
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string ForcerName { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string ForcerMark { get; set; }
        public bool ForcerSwitch { get; set; }
        public int ForcerUp { get; set; }
        public int ForcerDn { get; set; }
        public int ForcerSet { get; set; }

        public virtual ICollection<RollerBaseStation> RollerBaseStation { get; set; }

    }

}
