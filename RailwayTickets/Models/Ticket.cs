using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayTickets.Models;

public class Ticket
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string PassengerName { get; set; } = string.Empty;

    [Range(1, 9999)]
    public int SeatNumber { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public int TrainId { get; set; }

    [Required]
    public int FromStationId { get; set; }

    [Required]
    public int ToStationId { get; set; }

    public Train? Train { get; set; }
    public Station? FromStation { get; set; }
    public Station? ToStation { get; set; }
}

