using Crossdyne.Toolkit.Primitives;
using Glyph.Assets.Domain.Models;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<ProjectResponse>> GetAllAsync();
        Task<Maybe<Project>> GetProjectByCode(string projectCode);
    }
}