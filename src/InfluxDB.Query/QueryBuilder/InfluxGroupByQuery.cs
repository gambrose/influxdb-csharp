namespace InfluxDB.Query.QueryBuilder
{
    public class InfluxGroupByQuery<TValues, TGroupBy> : ISeriesQuery<TValues, TGroupBy>
    {
        public string Text => string.Empty;
    }
}