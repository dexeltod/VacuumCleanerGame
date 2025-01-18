using UnityEngine;

public interface ISceneGameObjects
{
	GameObject SpawnPoint { get; }
	GameObject SandContainer { get; }
	GameObject VirtualCamera { get; }
	GameObject MainCamera { get; }
	GameObject UpgradeTrigger { get; }
	GameObject SandGround { get; }
	GameObject CameraConfiner { get; }
}