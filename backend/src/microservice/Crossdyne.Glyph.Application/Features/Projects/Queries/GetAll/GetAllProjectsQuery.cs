using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Projects.Queries.GetAll
{
    public sealed record GetAllProjectsQuery() : IRequest<List<ProjectResponse>>;
}