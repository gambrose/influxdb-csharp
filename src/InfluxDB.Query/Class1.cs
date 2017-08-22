using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfluxDB.Query
{

    public class InfluxMeasurement<TTags, TFields>
    {
        public static InfluxSelectQuery<TValues, TTags, TFields> Select<TValues>(Expression<Func<TFields, TValues>> select)
        {
            return null;
        }

        public static InfluxSelectQuery<TValues, TTags, TFields> Select<TValues>(Expression<Func<TFields, TTags, TValues>> select)
        {
            return null;
        }

        public static InfluxSelectQuery<TFields, TTags, TFields> SelectAll()
        {
            return null;
        }
    }

    public class Series<TValues, TGroupBy>
    {
        public TGroupBy Tags { get; }

        public IList<(TValues values, DateTime time)> Points { get; }

    }

    public static class SeriesCollectionExtensions
    {
        public static IList<(TGroupBy tags, TValues values, DateTime time)> Flatten<TValues, TGroupBy>(this IList<Series<TValues, TGroupBy>> series)
        {
            return null;
        }
    }

    public class InfluxGroupByQuery<TValues, TGroupBy>
    {

    }

    public class InfluxSelectQuery<TValues, TTags, TFields>
    {
        public InfluxSelectQuery<TValues, TTags, TFields> Where(Expression<Func<DateTime, TFields, TTags, bool>> preticate)
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

        public InfluxGroupByQuery<TValues, TGroupBy> GroupBy<TGroupBy>(TimeSpan timeInterval, Expression<Func<TTags, TGroupBy>> select)
        {
            return null;
        }

        public InfluxGroupByQuery<TValues, TTags> GroupByAll()
        {
            return null;
        }
    }

    public class InfluxDb
    {
        public IList<(TValues values, DateTime time)> Query<TValues, TTags, TFields>(InfluxSelectQuery<TValues, TTags, TFields> query)
        {
            return null;
        }

        public IList<Series<TValues, TTags>> Query<TValues, TTags>(InfluxGroupByQuery<TValues, TTags> query)
        {
            return null;
        }
    }
}
