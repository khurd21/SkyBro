using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace WeatherObservations.Data.DynamoDB.Converters;

public class DateTimeConverter : IPropertyConverter
{
    public object FromEntry(DynamoDBEntry entry)
    {
        return entry.AsDateTime().ToUniversalTime();
    }

    public DynamoDBEntry ToEntry(object value)
    {
        return (DateTime)value;
    }
}