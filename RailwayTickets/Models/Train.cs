using System.ComponentModel.DataAnnotations;

namespace RailwayTickets.Models;

public class Train
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Number { get; set; } = string.Empty;

    [StringLength(120)]
    public string? Model { get; set; }

    [Range(10, 2000)]
    public int SeatCount { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

