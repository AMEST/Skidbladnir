using Microsoft.EntityFrameworkCore;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    internal interface IEntityTypeDefinition
    {
        void Configure(ModelBuilder modelBuilder);
    }
}