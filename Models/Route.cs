namespace lab013.Models;

public class Route
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty; // Например: "Москва - Санкт-Петербург"
	public string DepartureStation { get; set; } = string.Empty;
	public string ArrivalStation { get; set; } = string.Empty;
	public DateTime DepartureTime { get; set; }
	public DateTime ArrivalTime { get; set; }
	public decimal Price { get; set; }
	public int TrainId { get; set; }
	public Train? Train { get; set; }

	public override string ToString() => $"{Name} ({DepartureStation} - {ArrivalStation})";
}

