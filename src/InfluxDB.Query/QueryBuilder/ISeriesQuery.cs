namespace InfluxDB.Query.QueryBuilder
{
    public interface ISeriesQuery<TValues, TTags>
    {
        string Text { get; }
    }
}