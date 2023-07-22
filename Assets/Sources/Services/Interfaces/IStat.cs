using System;

namespace Sources.Services
{
	interface IStat
	{
		string Name { get; }
		int Value { get; }
		event Action Changed;
	}
}