namespace Crossdyne.Glyph.Domain.ValueObjects.Assets
{
    public readonly record struct Format
    {
        public int Value { get; }
        public string Name { get; } 

        public static readonly Format Svg   = new(1, ".svg");


        public static readonly IReadOnlyList<Format> All = [Svg];


        public static readonly HashSet<string> VectorFormats = new(StringComparer.OrdinalIgnoreCase)
        {
            ".svg"
        };

        private Format(int value, string name)
        {
            if (value <= 0) 
                throw new ArgumentException("Значение Format должно быть положительным.", nameof(value));
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Название Format не может быть пустым.", nameof(name));

            Value = value;
            Name = name.Trim().ToLowerInvariant();
        }

        public static Format FromName(string name)
        {
            var normalized = name.Trim().ToLowerInvariant();
            var format = All.FirstOrDefault(f => f.Name == normalized);
            
            if (format == default)
                throw new ArgumentException($"Формат '{name}' не входит в список разрешённых.", nameof(name));

            return format;
        }
        
        public static bool TryFromName(string name, out Format format)
        {
            format = default;
            if (string.IsNullOrWhiteSpace(name)) return false;

            var normalized = name.Trim().ToLowerInvariant();
            format = All.FirstOrDefault(f => f.Name == normalized);
            return format != default;
        }

        public static Format FromValue(int value)
        {
            var format = All.FirstOrDefault(f => f.Value == value);
            if (format == default)
                throw new ArgumentException($"Значение {value} не соответствует ни одному разрешённому формату.", nameof(value));
            return format;
        }

        public bool IsVector => VectorFormats.Contains(Name);
        public override string ToString() => Name;
    }
}