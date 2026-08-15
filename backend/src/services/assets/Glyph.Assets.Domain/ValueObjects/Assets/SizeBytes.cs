namespace Glyph.Assets.Domain.ValueObjects.Assets
{
    public readonly record struct SizeBytes: IComparable<SizeBytes>, IComparable<long>
    {
        public long Value { get; }

        public const long MaxSize = 50L * 1024 * 1024;

        private SizeBytes(long value)
        {
            Value = value;
        }

        public static SizeBytes Create(long value)
        {
            if (value < 0 || value > MaxSize)
                throw new ArgumentOutOfRangeException($"Значение не может быть меньше 0 и больше {MaxSize}");

            return new(value);
        }

        public double Kilobytes => Value / 1024.0;
        public double Megabytes => Value / (1024.0 * 1024.0);
        public double Gigabytes => Value / (1024.0 * 1024.0 * 1024.0);

        public string Humanity()
        {
            if (Value == 0)
                return "0 B";

            string[] units = ["B", "KB", "MB", "GB", "TB"];
            double len = Value;
            int index = 0;

            while (len >= 1024 && index < units.Length - 1)
            {
                index++;
                len /= 1024;
            }

            return FormattableString.Invariant($"{len:0.##} {units[index]}");
        }

        public int CompareTo(SizeBytes other) => Value.CompareTo(other.Value);
        public int CompareTo(long other) => Value.CompareTo(other);

        public static bool operator <(SizeBytes left, SizeBytes right) => left.Value < right.Value;
        public static bool operator >(SizeBytes left, SizeBytes right) => left.Value > right.Value;
        public static bool operator <=(SizeBytes left, SizeBytes right) => left.Value <= right.Value;
        public static bool operator >=(SizeBytes left, SizeBytes right) => left.Value >= right.Value;

        public static implicit operator long(SizeBytes size) => size.Value;
        public static implicit operator SizeBytes(long value) => new SizeBytes(value);

        public override string ToString() => Humanity();
    }
}