using NetCord;
using NetCord.Rest;
using System.Threading;

namespace Namerd.Application.Interfaces;

public interface IGuildUserWrapper : IUserWrapper
{
    public string? Nickname { get; }
    public ulong GuildId { get; }
    
    public Task<IGuildUserWrapper> ModifyAsync(
        Action<GuildUserOptions> action,
        RestRequestProperties? properties = null,
        CancellationToken cancellationToken = default);
}