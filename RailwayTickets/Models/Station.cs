using System.ComponentModel.DataAnnotations;

namespace RailwayTickets.Models;

public class Station
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string City { get; set; } = string.Empty;

    public ICollection<Ticket> DepartureTickets { get; set; } = new List<Ticket>();
    public ICollection<Ticket> ArrivalTickets { get; set; } = new List<Ticket>();
}

