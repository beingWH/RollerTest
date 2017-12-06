using RollerTest.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RollerTest.Domain.Concrete
{
    public class EFDbContext:DbContext
    {
        public EFDbContext() :base("ROLLERConnection") { }

        public DbSet<RollerBaseStation> RollerBaseStations { get; set; }
        public DbSet<RollerRecordInfo> RollerRecordInfos { get; set; }
        public DbSet<RollerSampleInfo> RollerSampleInfos { get; set; }
        public DbSet<RollerTestreportInfo> RollerTestreportInfos { get; set; }
        public DbSet<RollerProjectInfo> RollerProjectInfos { get; set; }
        public DbSet<TimerCfg> TimerCfgs { get; set; }
        public DbSet<ForcerCfg> ForcerCfgs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer<EFDbContext>(new DropCreateDatabaseIfModelChanges<EFDbContext>());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("ROLLER_WORK");
            base.OnModelCreating(modelBuilder);
        }
    }
}
