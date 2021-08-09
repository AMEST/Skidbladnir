using Skidbladnir.Repository.Abstractions;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities
{
    public class SimpleGuid : IHasId<int>
    {
        public int Id { get; set; }

        public string Guid { get; set; }
    }
}