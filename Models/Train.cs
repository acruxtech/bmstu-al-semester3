namespace lab013.Models;

public class Train
{
	public int Id { get; set; }
	public string Number { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty; // Скоростной, Пассажирский, Грузовой
	public int Capacity { get; set; } // Вместимость

	public override string ToString() => $"{Number} ({Type})";
}

