using System;

namespace AnimalsLib;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class CommentAttribute : Attribute
{
	public CommentAttribute(string comment)
	{
		Comment = comment;
	}

	public string Comment { get; }
}


