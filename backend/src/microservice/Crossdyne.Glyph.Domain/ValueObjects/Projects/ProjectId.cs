namespace Crossdyne.Glyph.Domain.ValueObjects.Projects
{
    public readonly record struct ProjectId
    {
        public Guid Value { get; }

        private ProjectId(Guid value) => Value = value;

        public static ProjectId From(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("ProjectId не может быть пустым.", nameof(value));

            return new ProjectId(value);
        }

        public static ProjectId New() => new(Guid.NewGuid());

        public static implicit operator Guid(ProjectId userId) => userId.Value;

        public override string ToString() => Value.ToString();
    }
}