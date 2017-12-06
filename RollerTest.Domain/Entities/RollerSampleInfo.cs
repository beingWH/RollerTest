using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RollerTest.Domain.Entities
{
    public class RollerSampleInfo:IValidatableObject
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
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
        [HiddenInput(DisplayValue = false), Column(TypeName = "varchar2"), MaxLength(100)]
        public string State { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<DateTime> StartTime { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<DateTime> EndTime { get; set; }
        public double TestTime { get; set; }
        public int TestID { get; set; }
        [HiddenInput(DisplayValue =false)]
        [Column(TypeName = "varchar2"), MaxLength(20)]
        public string TestTotalTime { get; set; }
        public virtual ICollection<RollerTestreportInfo> RollerTestreportInfo { get; set; }
        public virtual ICollection<RollerRecordInfo> RollerRecordInfo { get; set; }

        [ForeignKey("RollerProjectInfo"),HiddenInput(DisplayValue =false)]
        public int RollerProjectInfoID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public virtual RollerProjectInfo RollerProjectInfo { get; set; }

        [HiddenInput(DisplayValue = false)]
        [ForeignKey("RollerBaseStation")]
        [Required(ErrorMessage ="工位号不能为空")]
        public int RollerBaseStationID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public virtual RollerBaseStation RollerBaseStation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            RollerSampleInfo rollersampleinfo = validationContext.ObjectInstance as RollerSampleInfo;
            if (rollersampleinfo.SetValue > rollersampleinfo.UpLimit || rollersampleinfo.SetValue < rollersampleinfo.DnLimit)
            {
                yield return new ValidationResult("设定值不符合要求",new string[] { "SetValue" });
            }
            if (rollersampleinfo.UpLimit < rollersampleinfo.SetValue || rollersampleinfo.UpLimit < rollersampleinfo.DnLimit)
            {
                yield return new ValidationResult("上限值不符合要求", new string[] { "UpLimit" });
            }
            if (rollersampleinfo.DnLimit > rollersampleinfo.SetValue || rollersampleinfo.UpLimit < rollersampleinfo.DnLimit)
            {
                yield return new ValidationResult("下限值不符合要求", new string[] { "DnLimit" });
            }
            if (rollersampleinfo.RollerBaseStationID == 0)
            {
                yield return new ValidationResult("工位号不能为空", new string[] { "RollerBaseStationID" });
            }
        }
    }
    public class TestSampleInfo
    {
        private TestSampleInfo()
        {

        }
        private static TestSampleInfo instance;
        private static readonly object locker = new object();
        public List<RollerSampleInfo> rollersampleinfos = new List<RollerSampleInfo>();
        public static TestSampleInfo GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new TestSampleInfo();
                    }
                }
            }
            return instance;
        }

        public void AddTestSample(RollerSampleInfo rsi)
        {
                lock (rollersampleinfos)
                {
                    bool flag = false;
                    foreach(var p in rollersampleinfos)
                    {
                        if (p.RollerSampleInfoID==rsi.RollerSampleInfoID)
                        {
                            flag = true;
                        }
                    }
                    if (!flag) { this.rollersampleinfos.Add(rsi); }
                   
                }
        }
        public void RemoveSampleList(int RollerSampleInfoID)
        {
                lock (rollersampleinfos)
                {
                    int index=this.rollersampleinfos.FindIndex(x => x.RollerSampleInfoID == RollerSampleInfoID);
                    this.rollersampleinfos.RemoveAt(index);
                }
        }
        public void EditSampleList(RollerSampleInfo rsi)
        {
            lock (rollersampleinfos)
            {
                int index = this.rollersampleinfos.FindIndex(x => x.RollerSampleInfoID == rsi.RollerSampleInfoID);
                if (!index.Equals(-1))
                {
                    this.rollersampleinfos.RemoveAt(index);
                    this.rollersampleinfos.Add(rsi);
                }
 
            }
        }

    }

}
