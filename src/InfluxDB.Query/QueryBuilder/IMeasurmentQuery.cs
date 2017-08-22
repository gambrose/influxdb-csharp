namespace InfluxDB.Query.QueryBuilder
{
    public interface IMeasurmentQuery<TValues>
    {
        string Text { get; }
    }
}