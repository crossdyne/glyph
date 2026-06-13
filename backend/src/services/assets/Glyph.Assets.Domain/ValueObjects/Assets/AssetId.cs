namespace Glyph.Assets.Domain.ValueObjects.Assets
{
    public readonly record struct AssetId
    {
        public Guid Value { get; }

        private AssetId(Guid value) => Value = value;

        public static AssetId From(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("AssetId не может быть пустым.", nameof(value));

            return new AssetId(value);
        }

        public static AssetId New() => new(Guid.NewGuid());

        public static implicit operator Guid(AssetId value) => value.Value;

        public override string ToString() => Value.ToString();
    }
}