using System;

namespace Sources.Domain.Temp
{
	public interface IProgressEntity
	{
		int ConfigId { get; }
		int CurrentLevel { get; }
		void AddOneLevel();

		public event Action LevelChanged;
		public event Action PriceChanged;
	}
}