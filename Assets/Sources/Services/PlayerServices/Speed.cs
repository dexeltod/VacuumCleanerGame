using System;
using Sources.Services;

namespace Sources.View.SceneEntity
{
	public class Speed : IStat
	{
		private int _speed;

		public string Name => "Speed";

		public int Value
		{
			get => _speed;
			set
			{
				if (value < 0)
					throw new Exception("Value is negative");

				_speed = value;
			}
		}

		public event Action Changed;
	}
}