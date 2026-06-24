using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Projects.Query.GetAll
{
    public sealed record GetAllProjectsQuery() : IRequest<List<ProjectResponse>>;
}