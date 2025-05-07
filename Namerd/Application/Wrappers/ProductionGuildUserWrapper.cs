using Namerd.Application.Interfaces;
using NetCord;
using NetCord.Rest;

namespace Namerd.Application.Wrappers;

public class ProductionGuildUserWrapper : IGuildUserWrapper
{
    private readonly GuildUser _guildUser;

    public ProductionGuildUserWrapper(GuildUser guildUser)
    {
        _guildUser = guildUser;
    }

    public ulong Id => _guildUser.Id;
    public string? Nickname => _guildUser.Nickname;
    public ulong GuildId => _guildUser.GuildId;
    public string? Username => _guildUser.Username;

    public async Task<IGuildUserWrapper> ModifyAsync(
        Action<GuildUserOptions> action,
        RestRequestProperties? properties = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _guildUser.ModifyAsync(action, properties, cancellationToken);
        return new ProductionGuildUserWrapper(result);
    }
}