using Avalonia.Controls;
using Avalonia.Interactivity;
using lab013.Models;

namespace lab013;

public partial class PassengerDialog : Window
{
	public Passenger Passenger { get; set; }

	public PassengerDialog() : this(null)
	{
	}

	public PassengerDialog(Passenger? passenger)
	{
		InitializeComponent();
		Passenger = passenger ?? new Passenger();
		
		FirstNameTextBox.Text = Passenger.FirstName;
		LastNameTextBox.Text = Passenger.LastName;
		PassportNumberTextBox.Text = Passenger.PassportNumber;
		PhoneTextBox.Text = Passenger.Phone;
	}

	private void Ok_Click(object sender, RoutedEventArgs e)
	{
		Passenger.FirstName = FirstNameTextBox.Text ?? "";
		Passenger.LastName = LastNameTextBox.Text ?? "";
		Passenger.PassportNumber = PassportNumberTextBox.Text ?? "";
		Passenger.Phone = PhoneTextBox.Text ?? "";
		
		Close(true);
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		Close(false);
	}
}

