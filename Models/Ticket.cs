namespace lab013.Models;

public class Ticket
{
	public int Id { get; set; }
	public int RouteId { get; set; }
	public Route? Route { get; set; }
	public int PassengerId { get; set; }
	public Passenger? Passenger { get; set; }
	public DateTime PurchaseDate { get; set; }
	public string SeatNumber { get; set; } = string.Empty;
	public decimal Price { get; set; }
}

