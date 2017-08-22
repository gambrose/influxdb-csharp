using System;

namespace InfluxDB.Query.Tests
{
    public class h2o_feet : InfluxMeasurement<h2o_feet.Tags, h2o_feet.Fields>
    {
        public interface Tags
        {
            string location { get; }
        }

        public interface Fields
        {
            // Should be "level description" 
            string level_description { get; set; }

            double water_level { get; set; }
        }
    }

    public class SelectStatementTests
    {
        public void Select_all_fields_and_tags_from_a_single_measurement()
        {
            // The example of this is "SELECT * FROM h2o_feet";
            // This would project tags and fields into one obejct. We cannot support this in C# without createing a new type.
            // Instead query using group by (which is more effiecten on the wire) then flatten in the results when we itterate.

            var db = new InfluxDb();

            var resuts = db.Query(h2o_feet.SelectAll().GroupByAll());

            // Should map to "SELECT \"level description\",water_level FROM h2o_feet GROUP BY location"; 

            foreach (var (tags, values, time) in resuts.Flatten())
            {
                //tags.location;
                //values.water_level;
            }
        }

        public void Select_specific_tags_and_fields_from_a_single_measurement()
        {
            //   "SELECT \"level description\",location,water_level FROM h2o_feet";

            var query = h2o_feet.Select((fields, tags) => new { fields.level_description, tags.location, fields.water_level });

            var db = new InfluxDb();

            var resuts = db.Query(query);

            foreach (var (values, time) in resuts)
            {

            }
        }
    }

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
