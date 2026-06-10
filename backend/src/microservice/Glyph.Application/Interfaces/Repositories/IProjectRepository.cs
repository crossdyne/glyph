using Glyph.Domain.Models;
using Shared.Contracts.Responses;

namespace Glyph.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<ProjectResponse>> GetAllAsync();
    }
}