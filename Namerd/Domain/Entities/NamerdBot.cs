using System.ComponentModel.DataAnnotations;

namespace Namerd.Domain.Entities;

public class NamerdBot
{
    [Key, Required]
    public ulong GuildId { get; set; }
    public Settings Settings { get; set; }
    public List<MonthlyNomination> MonthlyNomination { get; set; } = new List<MonthlyNomination>();
}