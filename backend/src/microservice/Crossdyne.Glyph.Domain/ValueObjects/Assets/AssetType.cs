namespace Crossdyne.Glyph.Domain.ValueObjects.Assets
{
    public readonly record struct AssetType
    {
        public int Value { get; }
        public string Name { get; }

        public static readonly AssetType Svg = new(1, nameof(Svg));

        public static IReadOnlyList<AssetType> All = [Svg];

        private AssetType(int value, string name)
        {
            if (value <= 0) 
                throw new ArgumentException("Значение AssetType должно быть положительным.", nameof(value));
            
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("AssetType не может быть пустым.", nameof(name));

            Value = value;
            Name = name;
        }

        public static AssetType FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым.", nameof(name));

            var isType = All.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (!isType)
                throw new ArgumentException($"Неизвестное название AssetType: '{name}'", nameof(name));

            return All.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static AssetType FromValue(int value)
        {
            var isType = All.Any(t => t.Value == value);

            if (!isType)
                throw new ArgumentException($"Неизвестное значение AssetType: {value}", nameof(value));

            return All.FirstOrDefault(t => t.Value == value);
        }
        
        public override string ToString() => Name;
    }
}