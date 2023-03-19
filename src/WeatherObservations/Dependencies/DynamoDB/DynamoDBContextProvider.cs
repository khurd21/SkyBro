using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Ninject.Activation;
using WeatherObservations.Data.DynamoDB.Converters;

namespace WeatherObservations.Dependencies.DynamoDB;

internal class DateTimeUtcConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value) => (DateTime)value;

    public object FromEntry(DynamoDBEntry entry)
    {
        var dateTime = entry.AsDateTime();
        return dateTime.ToUniversalTime();
    }
}

public sealed class DynamoDBContextProvider : Provider<IDynamoDBContext>
{
    private AmazonDynamoDBClient Client { get; init; }

    private DynamoDBContextConfig Config { get; init; }

    public DynamoDBContextProvider(AmazonDynamoDBClient client, DynamoDBContextConfig config)
    {
        this.Client = client;
        this.Config = config;
    }

    protected override IDynamoDBContext CreateInstance(IContext context)
    {
        var dynamoContext = new DynamoDBContext(this.Client, this.Config);
        dynamoContext.ConverterCache.Add(typeof(DateTime), new DateTimeConverter());
        return dynamoContext;
    }
}