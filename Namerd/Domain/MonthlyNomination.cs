using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCord;

namespace Namerd.Domain;

public class MonthlyNomination
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public ulong botId { get; set; }
    public NamerdBot bot { get; set; }
    public List<NominationDetails> NominationDetails { get; set; } = new List<NominationDetails>();
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool IsNominationActive { get; set; }

    /*public MonthlyNominations()
    {
        Nominations = new();
        MonthStartDate = DateTime.UtcNow.AddDays(1 - DateTime.UtcNow.Day);
        MonthEndDate = 
    }
    
    private void UpdateDates */
}

public class NominationDetails
{
    [Key]
    public ulong UserId { get; set; }
    [MinLength(1), MaxLength(300)]
    public string Reason { get; set; } = string.Empty;
    public int Votes { get; set; }
}