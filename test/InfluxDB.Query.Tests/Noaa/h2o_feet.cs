using InfluxDB.Query.QueryBuilder;

namespace InfluxDB.Query.Tests.Noaa
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
}

