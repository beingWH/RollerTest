using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.Models.WTTESTMODEL
{
    public class WTSAMPLEINFO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长
        public int ID { get; set; }
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR2")]
        public string SERIAL_NO { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string NAME { get; set; }
        [MaxLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string DWG_NO { get; set; }
        public int COUNT { get; set; }
        [MaxLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string STATE { get; set; }
        public virtual ICollection<WTTESTINFO> TestInfos { get; set; }
    }
}
