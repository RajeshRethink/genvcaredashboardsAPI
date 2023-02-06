using genvcaredashboardsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using genvcaredashboardsAPI.Models.Event;

namespace genvcaredashboardsAPI.DBContexts
{
    public partial class GenVCareContext : DbContext
    {
        public GenVCareContext(DbContextOptions<GenVCareContext> options)
            : base(options)
        {
        }

       
       // public virtual DbSet<EventType> EventType { get; set; }

        public DbQuery<AllUniqueEventsAndTestsDTO> GetFetchAllUniqueEventsAndTests { get; set; }
        public DbQuery<AllUniqueAbnormalEventsAndTestsDTO> GetFetchAllUniqueAbnormalEventsAndTests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-JHENJGL;Initial Catalog=GenVCare;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
