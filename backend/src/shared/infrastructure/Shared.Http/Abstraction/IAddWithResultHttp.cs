namespace Shared.Http.Abstraction
{
    public interface IAddWithResultHttp
    {
        Task<TResult> AddWithResultAsync<TResult, TRequest>(TRequest request);
    }
}