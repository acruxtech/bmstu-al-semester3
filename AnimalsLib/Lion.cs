namespace AnimalsLib;

[Comment("Класс Лев")]
public class Lion : Animal
{
	public Lion() { Classification = eClassificationAnimal.Carnivores; }

	public Lion(string name, string country, bool hideFromOtherAnimals = false)
		: base(name, country, hideFromOtherAnimals, eClassificationAnimal.Carnivores) { }

	public override eFavouriteFood GetFavouriteFood()
	{
		return eFavouriteFood.Meat;
	}
}


