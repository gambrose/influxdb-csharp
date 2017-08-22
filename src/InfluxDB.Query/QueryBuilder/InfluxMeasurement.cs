using System;
using System.Linq.Expressions;

namespace InfluxDB.Query.QueryBuilder
{
    public class InfluxMeasurement<TTags, TFields>
    {
        public InfluxMeasurement(string name)
        {
        }

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
}