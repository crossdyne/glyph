namespace Shared.Http.Abstraction
{
    public interface IWriteHttpService<TResponse, in TKey> :
        IAddHttp<TResponse>,
        IAddWithResultHttp,
        ICreateHttp<TResponse>,
        IUpdateHttp<TKey>,
        IReplaceHttp<TKey>,
        IDeleteHttp<TKey>
        where TResponse : class
    {
        
    }
}