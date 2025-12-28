namespace lab013.Models;

public class Passenger
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PassportNumber { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;

	public override string ToString() => $"{LastName} {FirstName}";
}

