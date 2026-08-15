using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Projects.Queries.GetAll
{
    public sealed record GetAllProjectsQuery() : IRequest<List<ProjectResponse>>;
}