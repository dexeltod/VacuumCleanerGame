using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResouce
{
	public interface ISoftMinedResource
	{
		Color Color { get; }
		GameObject Prefab { get; }
		int Score { get; }
	}
}