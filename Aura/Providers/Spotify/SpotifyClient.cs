using Aura.Providers.Spotify.Endpoints;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Aura.Providers.Spotify;

public class SpotifyClient
{
    public string? ClientId { get; private set; }
    public string? ClientSecret { get; private set; }
    public PlayerEndpoint? Player { get; private set; }
    public TracksEndpoint? Tracks { get; private set; }

    private readonly HttpClient _httpClient;

    private string? _accessToken;

    public SpotifyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ConnectAsync(string clientId, string clientSecret)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;

        var code = await GetCodeAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "http://localhost:8888/" }
            })
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(clientId + ":" + clientSecret));

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadAsStringAsync();
        using (var document = JsonDocument.Parse(content))
        {
            var root = document.RootElement;

            if (root.TryGetProperty("access_token", out JsonElement accessTokenElement))
            {
                _accessToken = accessTokenElement.GetString();
            }
        }

        var result = !string.IsNullOrEmpty(_accessToken);

        if (result)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            Player = new PlayerEndpoint(_accessToken!, _httpClient);
            Tracks = new TracksEndpoint(_accessToken!, _httpClient);
        }

        return result;
    }

    private async Task<string> GetCodeAsync()
    {
        var authorizationEndpoint = $"https://accounts.spotify.com/authorize"
            + $"?client_id=" + ClientId
            + "&response_type=code"
            + "&redirect_uri=http://localhost:8888/"
            + "&scope=user-read-currently-playing";

        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();

        var code = string.Empty;

        var listenerTask = Task.Run(async () =>
        {
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;

                if (request.HttpMethod == "GET")
                {
                    code = ExtractQueryStringParameter(request, "code");
                    if (!string.IsNullOrEmpty(code))
                    {
                        Console.WriteLine($"code={code}");
                        listener.Stop();
                    }
                }
            }
        });

        Process.Start(new ProcessStartInfo
        {
            FileName = authorizationEndpoint,
            UseShellExecute = true
        });

        await listenerTask;

        return code;
    }

    private static string? ExtractQueryStringParameter(HttpListenerRequest request, string parameterName)
    {
        var query = request.QueryString;
        return query[parameterName];
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}