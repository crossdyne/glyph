using Glyph.Assets.Domain.Primitives;
using Glyph.Assets.Domain.ValueObjects.Projects;

namespace Glyph.Assets.Domain.Models
{
    public sealed class Project : AggregateRoot<ProjectId>
    {
        public ProjectName Name { get; private set; }

        private Project()
        {
            
        }

        private Project(ProjectName name) : base(ProjectId.New())
        {
            Name = name;
        }

        public static Project Create(ProjectName name) => new (name);

        public void UpdateName(ProjectName projectName)
        {
            Name = projectName;
        }
    }
}