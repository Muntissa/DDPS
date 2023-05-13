using DDPS.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDPS.Api
{
    public class HotelContext : DbContext
    {
        public DbSet<Apartaments> Apartaments => Set<Apartaments>();
        public DbSet<Bookings> Bookings => Set<Bookings>();
        public DbSet<Clients> Clients => Set<Clients>();
        public DbSet<Facilities> Facilities => Set<Facilities>();
        public DbSet<Services> Services => Set<Services>();
        public DbSet<Tariffs> Tariffs => Set<Tariffs>();
        public DbSet<BookingsArchive> BookingsArchive => Set<BookingsArchive>();

        public HotelContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        { 
            options.UseLazyLoadingProxies().UseSqlite("Data Source = HotelDB.db"); options.LogTo(message => System.Diagnostics.Debug.WriteLine(message)); 
        }
    }
}