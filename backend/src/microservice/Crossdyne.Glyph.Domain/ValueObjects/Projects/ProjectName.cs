namespace Crossdyne.Glyph.Domain.ValueObjects.Projects
{
    public readonly record struct ProjectName
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public string Value { get; }

        private ProjectName(string value) => Value = value;

        public static ProjectName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ProjectName не может быть пустым.", nameof(value));

            return new ProjectName(value);
        }

        public static implicit operator string(ProjectName value) => value;

        public override string ToString() => Value.ToString();
    }
}