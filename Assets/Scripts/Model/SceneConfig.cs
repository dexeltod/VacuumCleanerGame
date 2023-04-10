using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
	[CreateAssetMenu(fileName = "ConfigLevel_", menuName = "Data/Level/LevelConfig")]
	public class SceneConfig : ScriptableObject
	{
		[FormerlySerializedAs("_musicName")] [SerializeField] public string MusicName;
		[FormerlySerializedAs("_sceneName")] [SerializeField] public string SceneName;
		[FormerlySerializedAs("_isStopMusicBetweenScenes")] [SerializeField] public bool IsStopMusicBetweenScenes;

	}
}