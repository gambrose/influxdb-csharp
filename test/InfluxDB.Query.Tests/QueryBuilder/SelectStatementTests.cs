using System;
using System.Threading.Tasks;
using InfluxDB.Query.Client;
using InfluxDB.Query.QueryBuilder;
using InfluxDB.Query.Tests.Noaa;

namespace InfluxDB.Query.Tests.QueryBuilder
{
    public class SelectStatementTests
    {
        public async Task Select_all_fields_and_tags_from_a_single_measurement()
        {
            // The example of this is "SELECT * FROM h2o_feet";
            // This would project tags and fields into one obejct. We cannot support this in C# without createing a new type.
            // Instead query using group by (which is more effiecten on the wire) then flatten in the results when we itterate.

            var db = new InfluxDb();

            var resuts = await db.Query(WaterDepth.SelectAll().GroupByAll());

            // Should map to "SELECT \"level description\",water_level FROM h2o_feet GROUP BY location"; 

            foreach (var (tags, values, time) in resuts.Flatten())
            {
                Console.WriteLine($"{time} {values.level_description} {tags.location} {values.water_level}");
            }
        }

        public async Task Select_specific_tags_and_fields_from_a_single_measurement()
        {
            //   "SELECT \"level description\",location,water_level FROM h2o_feet";

            var query = WaterDepth.Select((fields, tags) => new { fields.level_description, tags.location, fields.water_level });

            var db = new InfluxDb();

            var resuts = await db.Query(query);

            foreach (var (values, time) in resuts)
            {

            }
        }
    }
}