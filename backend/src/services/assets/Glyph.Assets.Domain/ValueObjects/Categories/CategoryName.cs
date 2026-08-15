namespace Glyph.Assets.Domain.ValueObjects.Categories
{
    public readonly record struct CategoryName
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public string Value { get; }

        private CategoryName(string value) => Value = value;

        public static CategoryName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CategoryName не может быть пустым.", nameof(value));

            return new CategoryName(value);
        }

        public static implicit operator string(CategoryName value) => value;

        public override string ToString() => Value.ToString();
    }
}