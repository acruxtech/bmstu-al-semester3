namespace AnimalsLib;

[Comment("Класс Свинья")]
public class Pig : Animal
{
	public Pig() { Classification = eClassificationAnimal.Omnivores; }

	public Pig(string name, string country, bool hideFromOtherAnimals = false)
		: base(name, country, hideFromOtherAnimals, eClassificationAnimal.Omnivores) { }

	public override eFavouriteFood GetFavouriteFood()
	{
		return eFavouriteFood.Everything;
	}
}


