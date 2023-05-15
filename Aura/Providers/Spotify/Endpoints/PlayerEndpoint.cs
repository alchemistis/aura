using Aura.Providers.Spotify.Model.Response;
using System.Text.Json;

namespace Aura.Providers.Spotify.Endpoints;

public class PlayerEndpoint : SpotifyEndpoint
{
    private const string Endpoint = "https://api.spotify.com/v1/me/player";

    public PlayerEndpoint(string accessToken, HttpClient httpClient) : base(accessToken, httpClient)
    {
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
