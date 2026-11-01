using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API_LCKR.Converters
{
    public class HoraJsonConverter : JsonConverter<TimeOnly>
    {
        private static readonly string[] Formatos = { "HH:mm", "HH:mm:ss" };

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var texto = reader.GetString();

            if (TimeOnly.TryParseExact(texto, Formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var hora))
            {
                return hora;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm", CultureInfo.InvariantCulture));
        }
    }
}
