using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;

namespace Restaurantes.Infrastructure.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Restaurante> Restaurantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurante>().ToTable("restaurantes");
        }
    }
}
