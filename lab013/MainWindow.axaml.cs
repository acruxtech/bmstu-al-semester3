using Avalonia.Controls;
using Avalonia.Interactivity;
using lab013.Data;
using lab013.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;

namespace lab013;

public partial class MainWindow : Window
{
	private ObservableCollection<Train> _trainsCollection = new ObservableCollection<Train>();

	public MainWindow()
	{
		InitializeComponent();
		InitializeDatabase();
		this.Loaded += (s, e) => LoadData();
	}

	private void InitializeDatabase()
	{
		using var context = new RailwayContext();
		context.Database.EnsureCreated();
	}

	private RailwayContext CreateContext()
	{
		return new RailwayContext();
	}

	private void LoadData()
	{
		RefreshTrains_Click(null!, null!);
		RefreshRoutes_Click(null!, null!);
		RefreshPassengers_Click(null!, null!);
		RefreshTickets_Click(null!, null!);
	}

	#region Trains CRUD
	private void RefreshTrains_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var trains = context.Trains.AsNoTracking().ToList();
		var newCollection = new ObservableCollection<Train>(trains);
		
		TrainsGrid.ItemsSource = newCollection;
		_trainsCollection = newCollection;
	}

	private async void AddTrain_Click(object sender, RoutedEventArgs e)
	{
		var dialog = new TrainDialog();
		var result = await dialog.ShowDialog<bool>(this);
		if (result)
		{
			using var context = CreateContext();
			context.Trains.Add(dialog.Train);
			context.SaveChanges();
			RefreshTrains_Click(sender, e);
		}
	}

	private async void EditTrain_Click(object sender, RoutedEventArgs e)
	{
		if (TrainsGrid.SelectedItem == null)
		{
			await new Window { Content = new TextBlock { Text = "Выберите поезд для редактирования" }, Width = 300, Height = 100, Title = "Внимание" }.ShowDialog(this);
			return;
		}
		
		if (TrainsGrid.SelectedItem is Train train)
		{
			var trainCopy = new Train 
			{ 
				Id = train.Id, 
				Number = train.Number, 
				Type = train.Type, 
				Capacity = train.Capacity 
			};
			var dialog = new TrainDialog(trainCopy);
			var result = await dialog.ShowDialog<bool>(this);
			if (result)
			{
				using var context = CreateContext();
				var dbTrain = context.Trains.Find(train.Id);
				if (dbTrain != null)
				{
					dbTrain.Number = trainCopy.Number;
					dbTrain.Type = trainCopy.Type;
					dbTrain.Capacity = trainCopy.Capacity;
					context.SaveChanges();
				}
				RefreshTrains_Click(sender, e);
			}
		}
	}

	private void DeleteTrain_Click(object sender, RoutedEventArgs e)
	{
		if (TrainsGrid.SelectedItem == null)
			return;
			
		if (TrainsGrid.SelectedItem is Train train)
		{
			try
			{
				using var context = CreateContext();
				var dbTrain = context.Trains.Find(train.Id);
				if (dbTrain != null)
				{
					context.Trains.Remove(dbTrain);
					context.SaveChanges();
				}
				RefreshTrains_Click(sender, e);
			}
			catch
			{
				// Игнорируем ошибки удаления
			}
		}
	}
	#endregion

	#region Routes CRUD
	private void RefreshRoutes_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var routes = context.Routes
			.Include(r => r.Train)
			.AsNoTracking()
			.ToList();
		RoutesGrid.ItemsSource = new ObservableCollection<Route>(routes);
	}

	private async void AddRoute_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var dialog = new RouteDialog(context);
		var result = await dialog.ShowDialog<bool>(this);
		if (result)
		{
			context.Routes.Add(dialog.Route);
			context.SaveChanges();
			RefreshRoutes_Click(sender, e);
		}
	}

	private async void EditRoute_Click(object sender, RoutedEventArgs e)
	{
		if (RoutesGrid.SelectedItem == null)
			return;
			
		if (RoutesGrid.SelectedItem is Route route)
		{
			var routeCopy = new Route
			{
				Id = route.Id,
				Name = route.Name,
				DepartureStation = route.DepartureStation,
				ArrivalStation = route.ArrivalStation,
				DepartureTime = route.DepartureTime,
				ArrivalTime = route.ArrivalTime,
				Price = route.Price,
				TrainId = route.TrainId
			};
			using var context = CreateContext();
			var dialog = new RouteDialog(context, routeCopy);
			var result = await dialog.ShowDialog<bool>(this);
			if (result)
			{
				var dbRoute = context.Routes.Find(route.Id);
				if (dbRoute != null)
				{
					dbRoute.Name = routeCopy.Name;
					dbRoute.DepartureStation = routeCopy.DepartureStation;
					dbRoute.ArrivalStation = routeCopy.ArrivalStation;
					dbRoute.DepartureTime = routeCopy.DepartureTime;
					dbRoute.ArrivalTime = routeCopy.ArrivalTime;
					dbRoute.Price = routeCopy.Price;
					dbRoute.TrainId = routeCopy.TrainId;
					context.SaveChanges();
				}
				RefreshRoutes_Click(sender, e);
			}
		}
	}

	private void DeleteRoute_Click(object sender, RoutedEventArgs e)
	{
		if (RoutesGrid.SelectedItem == null)
			return;
			
		if (RoutesGrid.SelectedItem is Route route)
		{
			try
			{
				using var context = CreateContext();
				var dbRoute = context.Routes.Find(route.Id);
				if (dbRoute != null)
				{
					context.Routes.Remove(dbRoute);
					context.SaveChanges();
				}
				RefreshRoutes_Click(sender, e);
			}
			catch
			{
				// Игнорируем ошибки удаления
			}
		}
	}
	#endregion

	#region Passengers CRUD
	private void RefreshPassengers_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var passengers = context.Passengers.AsNoTracking().ToList();
		PassengersGrid.ItemsSource = new ObservableCollection<Passenger>(passengers);
	}

	private async void AddPassenger_Click(object sender, RoutedEventArgs e)
	{
		var dialog = new PassengerDialog();
		var result = await dialog.ShowDialog<bool>(this);
		if (result)
		{
			using var context = CreateContext();
			context.Passengers.Add(dialog.Passenger);
			context.SaveChanges();
			RefreshPassengers_Click(sender, e);
		}
	}

	private async void EditPassenger_Click(object sender, RoutedEventArgs e)
	{
		if (PassengersGrid.SelectedItem == null)
			return;
			
		if (PassengersGrid.SelectedItem is Passenger passenger)
		{
			var passengerCopy = new Passenger
			{
				Id = passenger.Id,
				FirstName = passenger.FirstName,
				LastName = passenger.LastName,
				PassportNumber = passenger.PassportNumber,
				Phone = passenger.Phone
			};
			var dialog = new PassengerDialog(passengerCopy);
			var result = await dialog.ShowDialog<bool>(this);
			if (result)
			{
				using var context = CreateContext();
				var dbPassenger = context.Passengers.Find(passenger.Id);
				if (dbPassenger != null)
				{
					dbPassenger.FirstName = passengerCopy.FirstName;
					dbPassenger.LastName = passengerCopy.LastName;
					dbPassenger.PassportNumber = passengerCopy.PassportNumber;
					dbPassenger.Phone = passengerCopy.Phone;
					context.SaveChanges();
				}
				RefreshPassengers_Click(sender, e);
			}
		}
	}

	private void DeletePassenger_Click(object sender, RoutedEventArgs e)
	{
		if (PassengersGrid.SelectedItem == null)
			return;
			
		if (PassengersGrid.SelectedItem is Passenger passenger)
		{
			try
			{
				using var context = CreateContext();
				var dbPassenger = context.Passengers.Find(passenger.Id);
				if (dbPassenger != null)
				{
					context.Passengers.Remove(dbPassenger);
					context.SaveChanges();
				}
				RefreshPassengers_Click(sender, e);
			}
			catch
			{
				// Игнорируем ошибки удаления
			}
		}
	}
	#endregion

	#region Tickets CRUD
	private void RefreshTickets_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var tickets = context.Tickets
			.Include(t => t.Route).ThenInclude(r => r!.Train)
			.Include(t => t.Passenger)
			.AsNoTracking()
			.ToList();
		TicketsGrid.ItemsSource = new ObservableCollection<Ticket>(tickets);
	}

	private async void AddTicket_Click(object sender, RoutedEventArgs e)
	{
		using var context = CreateContext();
		var dialog = new TicketDialog(context);
		var result = await dialog.ShowDialog<bool>(this);
		if (result)
		{
			context.Tickets.Add(dialog.Ticket);
			context.SaveChanges();
			RefreshTickets_Click(sender, e);
		}
	}

	private async void EditTicket_Click(object sender, RoutedEventArgs e)
	{
		if (TicketsGrid.SelectedItem == null)
			return;
			
		if (TicketsGrid.SelectedItem is Ticket ticket)
		{
			var ticketCopy = new Ticket
			{
				Id = ticket.Id,
				RouteId = ticket.RouteId,
				PassengerId = ticket.PassengerId,
				SeatNumber = ticket.SeatNumber,
				Price = ticket.Price,
				PurchaseDate = ticket.PurchaseDate
			};
			using var context = CreateContext();
			var dialog = new TicketDialog(context, ticketCopy);
			var result = await dialog.ShowDialog<bool>(this);
			if (result)
			{
				var dbTicket = context.Tickets.Find(ticket.Id);
				if (dbTicket != null)
				{
					dbTicket.RouteId = ticketCopy.RouteId;
					dbTicket.PassengerId = ticketCopy.PassengerId;
					dbTicket.SeatNumber = ticketCopy.SeatNumber;
					dbTicket.Price = ticketCopy.Price;
					dbTicket.PurchaseDate = ticketCopy.PurchaseDate;
					context.SaveChanges();
				}
				RefreshTickets_Click(sender, e);
			}
		}
	}

	private void DeleteTicket_Click(object sender, RoutedEventArgs e)
	{
		if (TicketsGrid.SelectedItem == null)
			return;
			
		if (TicketsGrid.SelectedItem is Ticket ticket)
		{
			try
			{
				using var context = CreateContext();
				var dbTicket = context.Tickets.Find(ticket.Id);
				if (dbTicket != null)
				{
					context.Tickets.Remove(dbTicket);
					context.SaveChanges();
				}
				RefreshTickets_Click(sender, e);
			}
			catch
			{
				// Игнорируем ошибки удаления
			}
		}
	}
	#endregion
}

