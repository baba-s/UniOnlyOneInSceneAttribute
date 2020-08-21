using System;

namespace Kogane
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class OnlyOneInSceneAttribute : Attribute
	{
	}
}