using System;
using System.Collections.Generic;

namespace InfluxDB.Query.Client
{
    public class Series<TValues, TGroupBy>
    {
        public TGroupBy Tags { get; }

        public IList<(TValues values, DateTime time)> Points { get; }

    }
}