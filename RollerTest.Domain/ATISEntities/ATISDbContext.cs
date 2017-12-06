using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RollerTest.Domain.ATISEntities;

namespace RollerTest.Domain.ATISEntities
{
    public class ATISDbContext:DbContext
    {
        public ATISDbContext() :base("name=ATISDEVConnection") { }

        public DbSet<ORollerBaseStation> RollerBaseStations { get; set; }
        public DbSet<ORollerRecordInfo> RollerRecordInfos { get; set; }
        public DbSet<ORollerSampleInfo> RollerSampleInfos { get; set; }
        public DbSet<ORollerTestreportInfo> RollerTestreportInfos { get; set; }
        public DbSet<ORollerProjectInfo> RollerProjectInfos { get; set; }
        public DbSet<OTimerCfg> TimerCfgs { get; set; }
        public DbSet<OForcerCfg> ForcerCfgs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ATISDbContext>());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("ATIS_DEV");
            base.OnModelCreating(modelBuilder);
        }
    }
}
