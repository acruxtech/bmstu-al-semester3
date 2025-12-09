using System.Xml.Serialization;
using AnimalsLib;

var filePath = args.Length > 0
	? args[0]
	: Path.Combine(AppContext.BaseDirectory, "animal.xml");

using var fs = new FileStream(filePath, FileMode.Open);
var animal = (Cow)new XmlSerializer(typeof(Cow)).Deserialize(fs)!;

Console.WriteLine($"{animal.WhatAnimal} {animal.Name} {animal.Country} {animal.Classification} {animal.GetFavouriteFood()}");
