using System.Xml.Serialization;
using AnimalsLib;

var animal = new Cow
{
	Name = "Burenka",
	Country = "RU",
	HideFromOtherAnimals = false
};

var filePath = Path.Combine(AppContext.BaseDirectory, "animal.xml");
using var fs = new FileStream(filePath, FileMode.Create);
new XmlSerializer(typeof(Cow)).Serialize(fs, animal);
Console.WriteLine(filePath);


