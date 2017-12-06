using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.Models.WTTESTMODEL
{
    public class WTTESTEQUIPMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长
        public int ID { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string NAME { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string TYPE { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string MNGNUM { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LOCATION { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string TESTTYPE { get; set; }
    }
}
