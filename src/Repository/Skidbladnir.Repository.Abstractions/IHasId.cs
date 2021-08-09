namespace Skidbladnir.Repository.Abstractions
{
    public interface IHasId<TId>
    {
        TId Id { get; set; }
    }
}