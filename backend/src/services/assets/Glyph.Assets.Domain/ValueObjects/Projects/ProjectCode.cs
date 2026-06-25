namespace Glyph.Assets.Domain.ValueObjects.Projects
{
    public readonly record struct ProjectCode
    {
        public string Value { get; }

        private ProjectCode(string value)
        {
            Value = value;
        }

        public static ProjectCode Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Код не должен быть пустым");

            return new ProjectCode(value);
        }

        public static implicit operator string(ProjectCode value) => value;

        public override string ToString() => Value.ToString();
    }
}