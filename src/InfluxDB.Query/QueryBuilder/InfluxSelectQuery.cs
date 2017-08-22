using System;
using System.Linq.Expressions;

namespace InfluxDB.Query.QueryBuilder
{
    public class InfluxSelectQuery<TValues, TTags, TFields> : IMeasurmentQuery<TValues>
    {
        public string Text => string.Empty;

        public InfluxSelectQuery<TValues, TTags, TFields> Where(
            Expression<Func<DateTime, TFields, TTags, bool>> preticate)
        {
            return null;
        }

        public InfluxSelectQuery<TValues, TTags, TFields> GroupBy(TimeSpan timeInterval)
        {
            return null;
        }

        public InfluxGroupByQuery<TValues, TGroupBy> GroupBy<TGroupBy>(Expression<Func<TTags, TGroupBy>> select)
        {
            return null;
        }

        public InfluxGroupByQuery<TValues, TGroupBy> GroupBy<TGroupBy>(TimeSpan timeInterval,
            Expression<Func<TTags, TGroupBy>> select)
        {
            return null;
        }

        public InfluxGroupByQuery<TValues, TTags> GroupByAll()
        {
            return null;
        }
    }
}