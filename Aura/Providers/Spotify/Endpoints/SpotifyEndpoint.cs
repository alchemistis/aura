namespace Aura.Providers.Spotify.Endpoints;

public class SpotifyEndpoint
{
    protected readonly string _accessToken;
    protected readonly HttpClient _httpClient;

    public SpotifyEndpoint(string accessToken, HttpClient httpClient)
    {
        _accessToken = accessToken;
        _httpClient = httpClient;
    }
}