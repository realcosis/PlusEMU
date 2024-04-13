using Newtonsoft.Json;

namespace Plus.HabboHotel.Items.Camera;

public class PhotoExtradata
{
    [JsonProperty("t")]
    public double? T { get; set; }

    [JsonProperty("w")]
    public string? W { get; set; }

    [JsonIgnore]
    public string? F { get; set; }

    [JsonProperty("n")]
    public string? N { get; set; }
}