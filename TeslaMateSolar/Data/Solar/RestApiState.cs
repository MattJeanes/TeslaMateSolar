using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TeslaMateSolar.Data.Solar;

public class RestApiState
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    [Required]
    [JsonPropertyName("solarWatts")]
    public int? SolarWatts { get; set; }

    [Required]
    [JsonPropertyName("loadWatts")]
    public int? LoadWatts { get; set; }

    [Required]
    [JsonPropertyName("gridOutWatts")]
    public int? GridOutWatts { get; set; }

    [Required]
    [JsonPropertyName("gridInWatts")]
    public int? GridInWatts { get; set; }
}
