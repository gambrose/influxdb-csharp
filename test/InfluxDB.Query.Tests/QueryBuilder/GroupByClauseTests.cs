using System;
using InfluxDB.Query.Client;
using InfluxDB.Query.QueryBuilder;
using InfluxDB.Query.Tests.Noaa;

namespace InfluxDB.Query.Tests
{
    public class GroupByClauseTests
    {
        public void Group_query_results_by_a_single_tag()
        {
            // SELECT MEAN("water_level") FROM "h2o_feet" GROUP BY "location"

            var query = h2o_feet.Select(fields => new { fields.water_level }).GroupBy(tags => new { tags.location });

            var db = new InfluxDb();

            var resuts = db.Query(query);

            foreach (var series in resuts)
            {
                //series.Tags.location;

                foreach (var (values, time) in series.Points)
                {
                    //values.water_level;
                }
            }
        }

        public void Group_query_results_into_12_minute_intervals()
        {
            // SELECT COUNT("water_level") FROM "h2o_feet" WHERE "location"='coyote_creek' AND time >= '2015-08-18T00:00:00Z' AND time <= '2015-08-18T00:30:00Z' GROUP BY time(12m)

            var query = h2o_feet.Select(fields => new { fields.water_level }).GroupBy(TimeSpan.FromMinutes(12));

            var db = new InfluxDb();

            var resuts = db.Query(query);

            foreach (var (values, time) in resuts)
            {
                //values.water_level;
            }
        }

        public void Group_query_results_into_12_minutes_intervals_and_by_a_tag_key()
        {
            // SELECT COUNT("water_level") FROM "h2o_feet" WHERE time >= '2015-08-18T00:00:00Z' AND time <= '2015-08-18T00:30:00Z' GROUP BY time(12m),"location"

            var query = h2o_feet.Select(fields => new { fields.water_level }).GroupBy(TimeSpan.FromMinutes(12), tags => new { tags.location });

            var db = new InfluxDb();

            var resuts = db.Query(query);

            foreach (var series in resuts)
            {
                //series.Tags.location;

                foreach (var (values, time) in series.Points)
                {
                    //values.water_level;
                }
            }
        }


    }
}