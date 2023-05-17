using Aura.Providers.Spotify.Model.Response;
using System.Text.Json;
using Aura.Json;

namespace Aura.Providers.Spotify.Endpoints;

public class PlayerEndpoint : SpotifyEndpoint
{
    private const string Endpoint = "https://api.spotify.com/v1/me/player";

    public PlayerEndpoint(string accessToken, HttpClient httpClient) : base(accessToken, httpClient)
    {
    }

    public async Task<GetPlaybackStateResponse?> GetPlaybackStateAsync()
    {
        var response = await _httpClient.GetAsync(Endpoint);
        
        var content = await response.Content.ReadAsStringAsync();
        var deserializedResponse = JsonSerializer.Deserialize<GetPlaybackStateResponse>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true
        });

        return deserializedResponse;
    }
    
    public async Task<GetCurrentlyPlayingTrackResponse?> GetCurrentTrackAsync()
    {
        var response = await _httpClient.GetAsync(Endpoint + "/currently-playing/");

        var content = await response.Content.ReadAsStringAsync();
        var deserializedResponse = JsonSerializer.Deserialize<GetCurrentlyPlayingTrackResponse>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        return deserializedResponse;
    }
}
