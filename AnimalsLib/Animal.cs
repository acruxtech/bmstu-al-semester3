using System.Xml.Serialization;

namespace AnimalsLib;

[Comment("Абстрактный базовый класс Животное")]
[XmlInclude(typeof(Cow))]
[XmlInclude(typeof(Lion))]
[XmlInclude(typeof(Pig))]
public abstract class Animal
{
	public string Name { get; set; }
	public string Country { get; set; }
	public bool HideFromOtherAnimals { get; set; }
	public string WhatAnimal => GetType().Name;
	public eClassificationAnimal Classification { get; set; }

	protected Animal() { }

	protected Animal(string name, string country, bool hideFromOtherAnimals, eClassificationAnimal classification)
	{
		Name = name;
		Country = country;
		HideFromOtherAnimals = hideFromOtherAnimals;
		Classification = classification;
	}

	public void SayHello()
	{
		System.Console.WriteLine($"Это животное {WhatAnimal} по имени {Name} из {Country}.");
	}

	public eClassificationAnimal GetClassificationAnimal()
	{
		return Classification;
	}

	public abstract eFavouriteFood GetFavouriteFood();
}


