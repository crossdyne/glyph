namespace Shared.Http.Abstraction
{
    public interface IReplaceHttp<in TKey>
    {
        Task ReplaceAsync<TRequest>(TKey id, TRequest request);
    }
}