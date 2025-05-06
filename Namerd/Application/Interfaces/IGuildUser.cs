using NetCord;
using NetCord.Rest;
using System.Threading;

namespace Namerd.Application.Interfaces;

public interface IGuildUser
{
    public ulong Id { get; }
    public string? Nickname { get; }
    public string? Username { get; }
    public ulong GuildId { get; }
    
    public Task<IGuildUser> ModifyAsync(
        Action<GuildUserOptions> action,
        RestRequestProperties? properties = null,
        CancellationToken cancellationToken = default);
}