using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

[Table("event_attendees")]
public class EventAttendees
{
    [Required]
    public int Event_Id { get; set; }
    [Required]
    public int Attendee_Id { get; set; }

    // Navigation properties with proper foreign key attributes
    [ForeignKey(nameof(Event_Id))]
    public Event Event { get; set; } = null!;
    [ForeignKey(nameof(Attendee_Id))]
    public User User { get; set; } = null!;
}