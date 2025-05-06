using Namerd.Domain.Entities;
using Namerd.Persistence.Context;

namespace Namerd.Persistence.Repository;

public class BotRepository
{
    private readonly NamerdContext _context;

    public BotRepository(NamerdContext context)
    {
        _context = context;
    }

    public async Task<NamerdBot> FindOrInitBot(ulong guildId)
    {
        var foundBot = await _context.Bots.FindAsync(guildId);
        if (foundBot == null)
        {
            var newBot = new NamerdBot()
            {
                GuildId = guildId,
                Settings = new Settings(),
            };
            _context.Bots.Add(newBot);
            await _context.SaveChangesAsync();
            return newBot;
        }

        return foundBot;
    }

    public async Task SetNominationChannel(ulong guildId ,ulong channelId)
    {
        var bot = await FindOrInitBot(guildId);
        if (bot.Settings.NominateChannelId != channelId)
        {
            bot.Settings.NominateChannelId = channelId;
            _context.Bots.Update(bot);
            await _context.SaveChangesAsync();
        }
    }
}