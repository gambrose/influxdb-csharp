using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfluxDB.Query.Client
{
    public class InfluxDb
    {
        public Task<IList<(TValues values, DateTime time)>> Query<TValues>(string query)
        {
            return null;
        }

        public Task<IList<Series<TValues, TTags>>> Query<TValues, TTags>(string query)
        {
            return null;
        }
    }
}