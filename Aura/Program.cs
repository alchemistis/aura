using Aura;
using Aura.Providers.Spotify;

DotEnv.Load(".env");
var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");

if (clientId is null || clientSecret is null)
{
    return;
}

using var httpClient = new HttpClient();

var spotifyClient = new SpotifyClient(httpClient);
if (!await spotifyClient.ConnectAsync(clientId, clientSecret))
{
    Console.Error.WriteLine("[ERROR]: Failed to Authorize with Spotify API...");
    return;
}

Console.WriteLine("Successfully authorized with Spotify API...");

var spotifyProvider = new SpotifyApiCurrentTrackProvider(spotifyClient);
var currentTrack = await spotifyProvider.GetCurrentTrackAsync();

if (currentTrack is null)
{
    return;
}

Console.WriteLine($"{currentTrack.Artist} - {currentTrack.Name}");
Console.WriteLine($"BPM: {currentTrack.Bpm}");