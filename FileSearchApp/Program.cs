using System.IO.Compression;

Console.Write("Путь: ");
var root = Console.ReadLine();
Console.Write("Файл: ");
var name = Console.ReadLine();

var file = Directory.GetFiles(root!, name!, SearchOption.AllDirectories).First();
using (var fs = new FileStream(file, FileMode.Open))
{
	fs.CopyTo(Console.OpenStandardOutput());
}

using var input = new FileStream(file, FileMode.Open);
using var gzip = new GZipStream(File.Create(file + ".gz"), CompressionLevel.Optimal);
input.CopyTo(gzip);
