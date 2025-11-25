using System.Reflection;
using System.Xml.Linq;
using AnimalsLib;

namespace Task1;

public static class Program
{
	public static void Main(string[] args)
	{
		var asm = typeof(Animal).Assembly;
		var root = new XElement("library", new XAttribute("name", asm.GetName().Name!));

		foreach (var type in asm.GetExportedTypes())
		{
			root.Add(type.IsEnum ? SerializeEnum(type) : SerializeClass(type));
		}

		new XDocument(root).Save(Path.Combine(AppContext.BaseDirectory, "diagram.xml"));
	}

	private static XElement SerializeClass(Type type)
	{
		var element = new XElement("class",
			new XAttribute("name", type.Name),
			new XAttribute("base", type.BaseType?.Name ?? string.Empty),
			new XAttribute("abstract", type.IsAbstract));

		var props = new XElement("properties");
		foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
		{
			props.Add(new XElement("property",
				new XAttribute("name", p.Name),
				new XAttribute("type", p.PropertyType.Name)));
		}
		element.Add(props);

		var methods = new XElement("methods");
		foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
		{
			if (m.IsSpecialName) continue;
			methods.Add(new XElement("method", new XAttribute("name", m.Name)));
		}
		element.Add(methods);

		return element;
	}

	private static XElement SerializeEnum(Type enumType)
	{
		var element = new XElement("enum",
			new XAttribute("name", enumType.Name));

		var names = Enum.GetNames(enumType);
		foreach (var n in names)
		{
			element.Add(new XElement("member", new XAttribute("name", n)));
		}
		return element;
	}
}
