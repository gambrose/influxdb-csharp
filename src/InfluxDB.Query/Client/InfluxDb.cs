using System;
using System.Collections.Generic;

namespace InfluxDB.Query.Client
{
    public class InfluxDb
    {
        public IList<(TValues values, DateTime time)> Query<TValues>(string query)
        {
            return null;
        }

        public IList<Series<TValues, TTags>> Query<TValues, TTags>(string query)
        {
            return null;
        }
    }
}