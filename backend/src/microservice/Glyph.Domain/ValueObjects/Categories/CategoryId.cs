namespace Glyph.Domain.ValueObjects.Categories
{
    public readonly record struct CategoryId
    {
        public Guid Value { get; }

        private CategoryId(Guid value) => Value = value;

        public static CategoryId From(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("CategoryId не может быть пустым.", nameof(value));

            return new CategoryId(value);
        }

        public static CategoryId New() => new(Guid.NewGuid());

        public static implicit operator Guid(CategoryId userId) => userId.Value;

        public override string ToString() => Value.ToString();
    }
}