using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities
{
    public class SimpleGuidConfiguration : IEntityTypeConfiguration<SimpleGuid>
    {
        public void Configure(EntityTypeBuilder<SimpleGuid> builder)
        {
            builder.ToTable("GuidsStorage");
        }
    }
}