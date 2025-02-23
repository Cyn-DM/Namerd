using System.ComponentModel.DataAnnotations;
using NetCord;
using NetCord.Gateway;

namespace Namerd.Domain;

public class NamerdBot
{
    [Key, Required]
    public ulong GuildId { get; set; }
    public Settings Settings { get; set; }
    public MonthlyNomination MonthlyNomination { get; set; }
}