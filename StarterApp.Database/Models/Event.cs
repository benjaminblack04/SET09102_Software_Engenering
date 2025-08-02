using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

[Table("event")]
[PrimaryKey(nameof(Id))]
public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    [Required]
    public int SpeakerId { get; set; }
    public DateTime Happening { get; set; } = DateTime.UtcNow;
    [ForeignKey(nameof(SpeakerId))]
    public User User { get; set; } = null!;
}