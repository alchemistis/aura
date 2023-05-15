using System.Net;
using System.Text;
using System.Text.Json;
using Aura.Api;
using Aura.Providers.Spotify;

namespace Aura;

public class AuraApiService
{
    private readonly ApiServer _server;
    private readonly SpotifyApiCurrentTrackProvider _spotifyCurrentTrackProvider;

    public AuraApiService(ApiServer server, SpotifyApiCurrentTrackProvider spotifyCurrentTrackProvider)
    {
        _server = server;
        _spotifyCurrentTrackProvider = spotifyCurrentTrackProvider;
        
        _server.RegisterEndpoint("/current", "GET", async (route, response) =>
        {
            Console.WriteLine($"Route {route} requested...");
            
            var currentTrack = await _spotifyCurrentTrackProvider.GetCurrentTrackAsync();
            
            if (currentTrack is null)
            {
                return;
            }
            
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            
            await using var output = response.OutputStream;

            var responseContent = JsonSerializer.Serialize(currentTrack);
            var buffer = Encoding.UTF8.GetBytes(responseContent);

            // Write the response content to the output stream
            output.Write(buffer, 0, buffer.Length);

            // Close the response
            response.Close();
            
            Console.WriteLine($"{currentTrack.Artist} - {currentTrack.Name}");
            Console.WriteLine($"BPM: {currentTrack.Bpm}");
        });
    }

    public async Task Start()
    {
        await _server.Start();
    }
}