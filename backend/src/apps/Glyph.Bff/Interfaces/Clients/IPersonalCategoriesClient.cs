using Shared.Contracts.Responses;
using Shared.Http.Abstraction;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IPersonalCategoriesClient : 
        IAddWithResultHttp, 
        IUpdateHttp<string>, 
        IDeleteHttp<string>, 
        IGetAllHttp<CategoryResponse>
    {
        Task<List<CategoryResponse>> GetAllPersonalAndGlobal();
    }
}