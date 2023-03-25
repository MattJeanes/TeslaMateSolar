using System.ComponentModel.DataAnnotations;

namespace TeslaMateSolar.Data.Options;

public class GrottOptions
{
    [Required]
    public string TopicName { get; set; }
}
