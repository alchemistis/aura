namespace Aura;

public interface ICurrentTrackProvider
{
    Task<Track?> GetCurrentTrackAsync();
}