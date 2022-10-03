using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ARM_Baidankino.Models
{
    public partial class DbConnect : DbContext
    {
        public DbConnect()
            : base("name=DbConnect")
        {
        }

        public virtual DbSet<OilCount> OilCounts { get; set; }
        public virtual DbSet<Alarm> Alarms { get; set; }
        public virtual DbSet<GazDay> GazDays { get; set; }
        public virtual DbSet<LevelHistory> LevelHistories { get; set; }
        public virtual DbSet<ManualOil> ManualOils { get; set; }
        public virtual DbSet<UserData> UserDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alarm>()
                .Property(e => e.AdoptUser)
                .IsUnicode(false);

            modelBuilder.Entity<Alarm>()
                .Property(e => e.AlarmType)
                .IsUnicode(false);

            modelBuilder.Entity<Alarm>()
                .Property(e => e.AlarmText)
                .IsUnicode(false);

            modelBuilder.Entity<Alarm>()
                .Property(e => e.Device)
                .IsUnicode(false);

            //modelBuilder.Entity<LevelHistory>()
            //    .Property(e => e.Tank)
            //    .IsUnicode(false);

            modelBuilder.Entity<ManualOil>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserData>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserData>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
