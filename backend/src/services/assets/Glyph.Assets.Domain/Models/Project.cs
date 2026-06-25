using Glyph.Assets.Domain.Primitives;
using Glyph.Assets.Domain.ValueObjects.Projects;

namespace Glyph.Assets.Domain.Models
{
    public sealed class Project : AggregateRoot<ProjectId>
    {
        public ProjectName Name { get; private set; }
        public ProjectCode Code { get; private set; }

        private Project()
        {
            
        }

        private Project(ProjectName name, ProjectCode code) : base(ProjectId.New())
        {
            Name = name;
            Code = code;
        }

        public static Project Create(ProjectName name, ProjectCode code) => new (name, code);

        public void UpdateName(ProjectName projectName)
        {
            Name = projectName;
        }
    }
}