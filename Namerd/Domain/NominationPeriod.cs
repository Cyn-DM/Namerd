using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCord;

namespace Namerd.Domain;

public class NominationPeriod
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ulong botId { get; set; }
    public NamerdBot bot { get; set; }
    public List<Nomination> NominationDetails { get; set; } = new List<Nomination>();
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    
}

