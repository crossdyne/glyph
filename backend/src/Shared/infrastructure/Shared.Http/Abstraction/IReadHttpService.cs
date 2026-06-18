namespace Shared.Http.Abstraction
{
    public interface IReadHttpService<TResponse, in TKey> :
    IGetAllHttp<TResponse>,
    IGetByIdHttp<TResponse, TKey> where TResponse : class
    {
        
    }
}