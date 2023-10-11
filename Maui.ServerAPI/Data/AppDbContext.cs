using Maui.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Maui.ServerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
