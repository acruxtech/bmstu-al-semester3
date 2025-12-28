using Avalonia.Controls;
using Avalonia.Interactivity;
using lab013.Models;

namespace lab013;

public partial class TrainDialog : Window
{
	public Train Train { get; set; }

	public TrainDialog() : this(null)
	{
	}

	public TrainDialog(Train? train)
	{
		InitializeComponent();
		Train = train ?? new Train();
		
		// Инициализация полей
		if (NumberTextBox != null)
			NumberTextBox.Text = Train.Number;
		if (CapacityTextBox != null)
			CapacityTextBox.Text = Train.Capacity.ToString();
		
		if (TypeComboBox != null && !string.IsNullOrEmpty(Train.Type))
		{
			foreach (ComboBoxItem? item in TypeComboBox.Items)
			{
				if (item?.Content?.ToString() == Train.Type)
				{
					TypeComboBox.SelectedItem = item;
					break;
				}
			}
		}
	}

	private void Ok_Click(object sender, RoutedEventArgs e)
	{
		Train.Number = NumberTextBox.Text ?? "";
		
		if (TypeComboBox.SelectedItem is ComboBoxItem item)
			Train.Type = item.Content?.ToString() ?? "";
		else
			Train.Type = "";
		
		if (int.TryParse(CapacityTextBox.Text, out int capacity))
			Train.Capacity = capacity;
		else
			Train.Capacity = 0;

		Close(true);
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		Close(false);
	}
}

