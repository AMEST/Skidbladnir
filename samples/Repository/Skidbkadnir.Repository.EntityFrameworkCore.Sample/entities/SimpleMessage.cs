using System;
using Skidbladnir.Repository.Abstractions;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities
{
    public class SimpleMessage : IHasId<int>
    {
        /// <inheritdoc />
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}