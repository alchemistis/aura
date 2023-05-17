namespace Aura.Providers.Spotify;

public class SpotifyApiCurrentTrackProvider : ICurrentTrackProvider
{
    private readonly SpotifyClient _spotifyClient;

    public SpotifyApiCurrentTrackProvider(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    public async Task<Track?> GetCurrentTrackAsync()
    {
        var currentTrack = await _spotifyClient.Player.GetCurrentTrackAsync();
        var audioFeatures = await _spotifyClient.Tracks.GetAudioFeatures(currentTrack.Item.Id);
        var playbackState = await _spotifyClient.Player.GetPlaybackStateAsync();

        var current = new Track(
            currentTrack.Item?.Name,
            currentTrack.Item?.Artists?[0].Name,
            audioFeatures.Tempo,
            playbackState.ProgressMs
        );
        
        return current;
    }
}