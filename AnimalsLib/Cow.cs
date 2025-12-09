namespace AnimalsLib;

[Comment("Класс Корова")]
public class Cow : Animal
{
	public Cow() { Classification = eClassificationAnimal.Herbivores; }

	public Cow(string name, string country, bool hideFromOtherAnimals = false)
		: base(name, country, hideFromOtherAnimals, eClassificationAnimal.Herbivores) { }

	public override eFavouriteFood GetFavouriteFood()
	{
		return eFavouriteFood.Plants;
	}
}


