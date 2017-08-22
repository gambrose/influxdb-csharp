A type safe influxdb query client.

# Clinet Api

## Query a single series 
```
public class WaterTemperature{
	public string location {get; set;}
	public double degrees {get; set;}
}

var db = new InfluxDb("NOAA_water_database");

var results = await db.Query<WaterTemperature>("SELECT degrees,location FROM h2o_temperature");

foreach (var (values, time) in resuts)
{
	Console.WriteLine($"{values.location} {values.degrees} {time}");
}
```
Each point is returned as a value tuple of values and time as a `DateTime`.
We are using tuple destucturing in the foreach to print out the values and time.

Values are matched with columns based on propery names. Attribues could be used to customise the name matching.


## Query multible series (GROUP BY)
```
public class WaterQuality{
	public class Fields{
		public double index {get; set;}
	}

	public class Tags{
		public string location {get; set;}
		public string randtag {get; set;}
	}
}

var results = await db.Query<WaterTemperature.Fields, WaterTemperature.Tags>("SELECT index FROM h2o_quality GROUP BY location,randtag");

foreach (var series in resuts)
{
	foreach (var (values, time) in series.Points)
    {
		Console.WriteLine($"{series.Tags.location} {series.Tags.randtag} {values.index} {time}");
	}
}
```
We are passing in Value and Tag types so we know this returns multi series.
Tag values are returned once for each series rather than for each point which is more efficient on the wire.

We can also flatten the results to make it more consise.
```
foreach (var (tags, values, time) in resuts.Flatten())
{
	Console.WriteLine($"{tags.location} {tags.randtag} {values.index} {time}");
}
```

# Design
The basic design of the query api is to use seperate types that define the values and tags a query has.

This seams overly verbose but has benifits when building queries using a fluent api and still enabling full intellisense for the results. As we can track projections made to the values and tags types. 

C# does not allow union types but by using an anonymous type in the select clause we can conbine both fields and tags into a single values type.
```
// Measurement type defintion used to build query projections.
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

var query = WaterDepth.Select((fields, tags) => new { fields.level_description, tags.location });

foreach (var (values, time) in resuts)
{
	// Complie error as water_level not selected. 
	Console.WriteLine($"{values.location} {values.level_description} {values.water_level} {time}");
} 
```

The Select function takes an `Expression` so that we can parse the expression tree and produce the select clause.
In this case we are importing the InfluxAggregations.COUNT function this is just a place holder that accepts field value types, in this case double, and returns int. 
```
using static InfluxAggregations;

var query = WaterDepth.Select(fields => new { count = COUNT(fields.water_level) }).GroupBy(tags => new { tags.location });

foreach (var (tags, values, time) in resuts.Flatten())
{
    Console.WriteLine($"{tags.location} {values.count} {time}");
}
```

Group by time clause is kept sperate from the tag selection. I haven't dsesigned the Where clause yet.
```
var query = WaterDepth.Select(fields => new { count = COUNT(fields.water_level) }).GroupBy(TimeSpan.FromMinutes(12), tags => new { tags.location });
```

This api enables a measurment to have a tag and a value with the same name. I don't know why you would want this but it does and influx supports this.

As we have the column types we can be more efficient when parsing the json results.