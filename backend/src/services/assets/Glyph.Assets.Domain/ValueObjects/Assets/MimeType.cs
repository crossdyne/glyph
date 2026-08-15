using System.Text.RegularExpressions;

namespace Glyph.Assets.Domain.ValueObjects.Assets
{
    public readonly record struct MimeType
    {
        private const string SvgValue = "image/svg+xml";

        public string Value { get; }

        private MimeType(string value)
        {
            Value = value.ToLowerInvariant();
        }

        public static MimeType Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("MIME тип не может быть пустым.");

            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9!#$&\-^_\.]+/[a-zA-Z0-9!#$&\-^_\.+]+$"))
                throw new FormatException($"Invalid MIME type format: '{value}'");

            return new(value);
        }

        public bool IsSvg => Value == SvgValue;
        public static MimeType Svg => new(SvgValue);

        public static MimeType FromFormat(Format format) => format.Name switch
        {
            ".svg" => Svg,
            _ => throw new FormatException($"MIME конвертация не поддерживается для данного формата: '{format.Value}'")
        };
    }
}