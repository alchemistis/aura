using Aura.Providers.Spotify.Model.Response;
using System.Text.Json;

namespace Aura.Providers.Spotify.Endpoints;

public class TracksEndpoint : SpotifyEndpoint
{
    private const string Endpoint = "https://api.spotify.com/v1/me/player";
    private const string AudioFeaturesEndpoint = "https://api.spotify.com/v1/audio-features";

    public TracksEndpoint(string accessToken, HttpClient httpClient) : base(accessToken, httpClient)
    {
    }

    public async Task<AudioFeaturesResponse?> GetAudioFeatures(string trackId)
    {
        var response = await _httpClient.GetAsync(AudioFeaturesEndpoint + "/" + trackId);
        var deserializedResponse = JsonSerializer.Deserialize<AudioFeaturesResponse>(response.Content.ReadAsStream(), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive= true
        });

        return deserializedResponse;
    }
}
