namespace Shared.Http.Abstraction
{
    public interface IHttpService<TResponse, in TKey> : IWriteHttpService<TResponse, TKey>, IReadHttpService<TResponse, TKey>
    where TResponse : class
    {
        
    }
}