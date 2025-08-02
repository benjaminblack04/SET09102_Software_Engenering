using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

[Table("event_attendees")]
public class EventAttendees
{
    [Required]
    public int EventId { get; set; }
    [Required]
    public int UserId { get; set; }

    // Navigation properties with proper foreign key attributes
    [ForeignKey(nameof(EventId))]
    public Event Event { get; set; } = null!;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}