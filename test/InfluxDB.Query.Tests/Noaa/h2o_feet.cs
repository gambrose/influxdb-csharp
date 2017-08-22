using InfluxDB.Query.QueryBuilder;

namespace InfluxDB.Query.Tests.Noaa
{
    public class WaterDepth : InfluxMeasurement<WaterDepth.Tags, WaterDepth.Fields>
    {
        public WaterDepth() : base("h2o_feet")
        {
        }

        public interface Tags
        {
            string location { get; }
        }

        public interface Fields
        {
            [InfluxKeyName("level description")]
            string level_description { get; set; }

            double water_level { get; set; }
        }
    }
}

