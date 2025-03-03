using UnityEditor;
using UnityEngine;

namespace Sources.Utils.Helpers
{
	public class CheckGPUMemory : MonoBehaviour
	{
		[MenuItem("Tools/Check GPU Memory")]
		private static void CheckMemory()
		{
			long availableMemory = SystemInfo.graphicsMemorySize;
			Debug.Log("Available GPU Memory: " + availableMemory + " MB");
		}
	}
}
