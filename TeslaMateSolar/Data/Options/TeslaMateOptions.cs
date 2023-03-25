using System.ComponentModel.DataAnnotations;

namespace TeslaMateSolar.Data.Options;

public class TeslaMateOptions
{
    [Required]
    public int? CarId { get; set; }
}
