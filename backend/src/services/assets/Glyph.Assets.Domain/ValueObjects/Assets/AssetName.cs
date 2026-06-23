namespace Glyph.Assets.Domain.ValueObjects.Assets
{
    public readonly record struct AssetName
    {
        public string Value { get; }

        private AssetName(string value) => Value = value;

        public static AssetName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название для ассета должно быть указано.", nameof(value));

            return new AssetName(value);
        }
        public static implicit operator string(AssetName value) => value.Value;

        public override string ToString() => Value.ToString();
    }
}