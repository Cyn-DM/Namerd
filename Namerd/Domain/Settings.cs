using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namerd.Domain;

public class Settings
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public ulong NominateChannelId { get; set; }
    public DateTime NominateEndTimeUTC { get; set; }
}