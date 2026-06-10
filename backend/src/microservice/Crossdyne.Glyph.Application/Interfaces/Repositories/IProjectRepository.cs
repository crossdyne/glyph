using Crossdyne.Glyph.Domain.Models;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<ProjectResponse>> GetAllAsync();
    }
}