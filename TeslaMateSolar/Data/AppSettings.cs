using System.ComponentModel.DataAnnotations;

namespace TeslaMateSolar.Data;

public class AppSettings
{
    [Required]
    public string MqttConnectionUri { get; set; }

    [Required]
    public int? CarId { get; set; }
}
