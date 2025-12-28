using Avalonia.Controls;
using Avalonia.Interactivity;
using lab013.Data;
using lab013.Models;
using Microsoft.EntityFrameworkCore;

namespace lab013;

public partial class TicketDialog : Window
{
	private RailwayContext _context;
	public Ticket Ticket { get; set; }

	public TicketDialog() : this(new RailwayContext(), null)
	{
	}

	public TicketDialog(RailwayContext context, Ticket? ticket = null)
	{
		InitializeComponent();
		_context = context;
		Ticket = ticket ?? new Ticket { PurchaseDate = DateTime.Now };
		
		SeatNumberTextBox.Text = Ticket.SeatNumber;
		PriceTextBox.Text = Ticket.Price.ToString();
		if (Ticket.PurchaseDate != default)
			PurchaseDatePicker.SelectedDate = Ticket.PurchaseDate;
		
		// Загружаем маршруты и пассажиров после загрузки окна
		this.Loaded += (s, e) =>
		{
			using var loadContext = new RailwayContext();
			var routesList = loadContext.Routes.Include(r => r.Train).ToList();
			var passengersList = loadContext.Passengers.ToList();
			
			RouteComboBox.ItemsSource = routesList;
			PassengerComboBox.ItemsSource = passengersList;
			RouteCountText.Text = $"Доступно маршрутов: {routesList.Count}";
			PassengerCountText.Text = $"Доступно пассажиров: {passengersList.Count}";
			
			if (Ticket.RouteId > 0)
			{
				var selectedRoute = routesList.FirstOrDefault(r => r.Id == Ticket.RouteId);
				if (selectedRoute != null)
					RouteComboBox.SelectedItem = selectedRoute;
			}
			else if (routesList.Count > 0)
			{
				RouteComboBox.SelectedItem = routesList.First();
			}
			
			if (Ticket.PassengerId > 0)
			{
				var selectedPassenger = passengersList.FirstOrDefault(p => p.Id == Ticket.PassengerId);
				if (selectedPassenger != null)
					PassengerComboBox.SelectedItem = selectedPassenger;
			}
			else if (passengersList.Count > 0)
			{
				PassengerComboBox.SelectedItem = passengersList.First();
			}
		};
	}

	private void Ok_Click(object sender, RoutedEventArgs e)
	{
		if (RouteComboBox.SelectedItem is Route route)
			Ticket.RouteId = route.Id;

		if (PassengerComboBox.SelectedItem is Passenger passenger)
			Ticket.PassengerId = passenger.Id;

		if (decimal.TryParse(PriceTextBox.Text, out decimal price))
			Ticket.Price = price;

		if (PurchaseDatePicker.SelectedDate.HasValue)
			Ticket.PurchaseDate = PurchaseDatePicker.SelectedDate.Value.DateTime;
		else
			Ticket.PurchaseDate = DateTime.Now;

		if (Ticket.Id == 0)
			_context.Tickets.Add(Ticket);
		else
			_context.Tickets.Update(Ticket);

		Close(true);
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		Close(false);
	}
}

