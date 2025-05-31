using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCord;

namespace Namerd.Domain;

public class NominationPeriod
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public ulong botId { get; set; }
    [Required]
    public NamerdBot bot { get; set; }
    public List<Nomination> Nominations { get; set; } = new List<Nomination>();
    [Required]
    public DateTime StartDateTime { get; set; }
    [Required]
    public DateTime EndDateTime { get; set; }
    
    private NominationPeriod() { }
    
}

