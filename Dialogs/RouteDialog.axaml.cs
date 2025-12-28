using Avalonia.Controls;
using Avalonia.Interactivity;
using lab013.Data;
using lab013.Models;
using Microsoft.EntityFrameworkCore;

namespace lab013;

public partial class RouteDialog : Window
{
	private RailwayContext _context;
	public Route Route { get; set; }

	public RouteDialog() : this(new RailwayContext(), null)
	{
	}

	public RouteDialog(RailwayContext context, Route? route = null)
	{
		InitializeComponent();
		_context = context;
		Route = route ?? new Route 
		{ 
			DepartureTime = DateTime.Now, 
			ArrivalTime = DateTime.Now.AddHours(2) 
		};
		
		NameTextBox.Text = Route.Name;
		DepartureStationTextBox.Text = Route.DepartureStation;
		ArrivalStationTextBox.Text = Route.ArrivalStation;
		DepartureTimeTextBox.Text = Route.DepartureTime.ToString("yyyy-MM-dd HH:mm");
		ArrivalTimeTextBox.Text = Route.ArrivalTime.ToString("yyyy-MM-dd HH:mm");
		PriceTextBox.Text = Route.Price.ToString();
		
		// Загружаем поезда после загрузки окна
		this.Loaded += (s, e) =>
		{
			using var loadContext = new RailwayContext();
			var trainsList = loadContext.Trains.ToList();
			TrainComboBox.ItemsSource = trainsList;
			TrainCountText.Text = $"Доступно поездов: {trainsList.Count}";
			
			if (Route.TrainId > 0)
			{
				var selectedTrain = trainsList.FirstOrDefault(t => t.Id == Route.TrainId);
				if (selectedTrain != null)
					TrainComboBox.SelectedItem = selectedTrain;
			}
			else if (trainsList.Count > 0)
			{
				TrainComboBox.SelectedItem = trainsList.First();
			}
		};
	}

	private void Ok_Click(object sender, RoutedEventArgs e)
	{
		Route.Name = NameTextBox.Text ?? "";
		Route.DepartureStation = DepartureStationTextBox.Text ?? "";
		Route.ArrivalStation = ArrivalStationTextBox.Text ?? "";
		
		if (TrainComboBox.SelectedItem is Train train)
			Route.TrainId = train.Id;

		if (decimal.TryParse(PriceTextBox.Text, out decimal price))
			Route.Price = price;

		if (DateTime.TryParse(DepartureTimeTextBox.Text, out DateTime departureTime))
			Route.DepartureTime = departureTime;

		if (DateTime.TryParse(ArrivalTimeTextBox.Text, out DateTime arrivalTime))
			Route.ArrivalTime = arrivalTime;

		if (Route.Id == 0)
			_context.Routes.Add(Route);
		else
			_context.Routes.Update(Route);

		Close(true);
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		Close(false);
	}
}

