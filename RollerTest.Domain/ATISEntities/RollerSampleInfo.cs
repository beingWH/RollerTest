using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RollerTest.Domain.ATISEntities
{
    public class ORollerSampleInfo
    {
        [Key]
        public int RollerSampleInfoID { get; set; }

        [Required(ErrorMessage ="样品编号不能为空"),Column(TypeName ="varchar2"),MaxLength(100)]
        public string SampleID { get; set; }
        [Required(ErrorMessage = "样品名称不能为空"), Column(TypeName = "varchar2"), MaxLength(100)]
        public string SampleName { get; set; }
        [Required(ErrorMessage ="试验类型不能为空"), Column(TypeName = "varchar2"), MaxLength(100)]
        public string TestType { get; set; }
        [Required]
        public int UpLimit { get; set; }
        [Required]
        public int DnLimit { get; set; }
        [Required]
        public int SetValue { get; set; }
        [Column(TypeName = "nvarchar2"), MaxLength(100)]
        public string State { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public double TestTime { get; set; }
        public string TestTotalTime { get; set; }
        public virtual ICollection<ORollerTestreportInfo> RollerTestreportInfo { get; set; }
        public virtual ICollection<ORollerRecordInfo> RollerRecordInfo { get; set; }

        [ForeignKey("RollerProjectInfo")]
        public int RollerProjectInfoID { get; set; }
        public virtual ORollerProjectInfo RollerProjectInfo { get; set; }

        [ForeignKey("RollerBaseStation")]
        [Required(ErrorMessage ="工位号不能为空")]
        public int RollerBaseStationID { get; set; }
        public virtual ORollerBaseStation RollerBaseStation { get; set; }

    }
 

}
