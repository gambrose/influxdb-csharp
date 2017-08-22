using System;
using System.Collections.Generic;
using InfluxDB.Query.Client;

namespace InfluxDB.Query.QueryBuilder
{
    public static class ClientExtentions
    {
        public static IList<(TValues values, DateTime time)> Query<TValues>(this InfluxDb client, IMeasurmentQuery<TValues> query)
        {
            return client.Query<TValues>(query.Text);
        }

        public static IList<Series<TValues, TTags>> Query<TValues, TTags>(this InfluxDb client, ISeriesQuery<TValues, TTags> query)
        {
            return client.Query<TValues, TTags>(query.Text);
        }
    }
}