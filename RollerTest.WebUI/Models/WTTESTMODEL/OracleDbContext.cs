using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.Models.WTTESTMODEL
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext()
            : base("name=LISDEVConnection")
        {
        }

        public virtual DbSet<WTATTACHMENT> WTAttachments { get; set; }
        public virtual DbSet<WTSAMPLEINFO> WTSampleInfos { get; set; }
        public virtual DbSet<WTTESTEQUIPMENT> WTTestEquipments { get; set; }
        public virtual DbSet<WTTESTINFO> WTTestInfos { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OracleDbContext>());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("LIS_DEV");
            base.OnModelCreating(modelBuilder);

        }
    }
}
