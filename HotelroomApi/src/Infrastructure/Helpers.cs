using Microsoft.Data.SqlClient;

namespace Infrastructure.Helpers;

internal static class DataReaderExtensions
{
    internal static bool? GetNullableBool(this SqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) 
            ? (bool?)null 
            : reader.GetBoolean(ordinal);
    }

    internal static int? GetNullableInt(this SqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) 
            ? (int?)null 
            : reader.GetInt32(ordinal);
    }
}
