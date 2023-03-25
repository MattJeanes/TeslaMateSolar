namespace TeslaMateSolar.Data;

public class SolarState
{
    public DateTimeOffset Timestamp { get; set; }
    public int SolarWatts { get; set; }
    public int LoadWatts { get; set; }
    public int GridOutWatts { get; set; }
    public int GridInWatts { get; set; }
}
