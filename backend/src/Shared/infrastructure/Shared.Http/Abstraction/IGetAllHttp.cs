namespace Shared.Http.Abstraction
{
    public interface IGetAllHttp<TResponse> where TResponse : class
    {
        Task<List<TResponse>> GetAllAsync();
    }
}