using Microsoft.EntityFrameworkCore;
using Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities;
using Skidbladnir.Repository.EntityFrameworkCore;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample
{
    public class SampleDbContext : DbContextBase
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        public DbSet<SimpleMessage> Messages { get; set; }

        public DbSet<SimpleGuid> Guids { get; set; }
    }
}