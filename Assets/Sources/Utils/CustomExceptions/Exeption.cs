using System;

namespace Sources.Utils.CustomExceptions
{
	public class MaxCountException : Exception
	{
		public MaxCountException(string message = "Count is out of range") : base(message)
		{
		}

		public MaxCountException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}