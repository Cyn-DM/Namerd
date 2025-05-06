using Namerd.Application.Interfaces;
using NetCord;
using NetCord.Rest;

namespace Namerd.Application.Wrappers;

public class ProductionGuildUser : IGuildUser
{
    private readonly GuildUser _guildUser;

    public ProductionGuildUser(GuildUser guildUser)
    {
        _guildUser = guildUser;
    }

    public ulong Id => _guildUser.Id;
    public string? Nickname => _guildUser.Nickname;
    public ulong GuildId => _guildUser.GuildId;
    public string? Username => _guildUser.Username;

    public async Task<IGuildUser> ModifyAsync(
        Action<GuildUserOptions> action,
        RestRequestProperties? properties = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _guildUser.ModifyAsync(action, properties, cancellationToken);
        return new ProductionGuildUser(result);
    }
}