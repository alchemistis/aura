using System.Text.Json.Serialization;

namespace Aura.Providers.Spotify.Model.Response;

public class GetPlaybackStateResponse
{
    [JsonPropertyName("is_playing")]
    public bool IsPlaying { get; set; }
    
    [JsonPropertyName("progress_ms")]    
    public int ProgressMs { get; set; }
}