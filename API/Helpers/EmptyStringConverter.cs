namespace API.Helpers;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

// Custom JsonConverter to handle empty strings
public class EmptyStringConverter<T> : JsonConverter<T>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(T);
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && reader.GetString() == string.Empty)
        {
            // Return an empty list if the string is empty
            return GetEmptyList();
        }

        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }

    private T GetEmptyList()
    {
        // Return an empty list of type T
        return Activator.CreateInstance<T>();
    }
}