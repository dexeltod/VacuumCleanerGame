using UnityEngine;

namespace Sources.Utils.Configs
{
	[CreateAssetMenu(fileName = "ConfigLevel_", menuName = "Data/Level/LevelConfig")]
	public class SceneConfig : ScriptableObject
	{
		[SerializeField] public string MusicName;
		[SerializeField] public string SceneName;
		[SerializeField] public bool IsStopMusicBetweenScenes = false;
	}
}