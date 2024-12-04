using UnityEditor;
using UnityEngine;

namespace Sources.Utils.ParticleColorChanger.Scripts
{
#if UNITY_EDITOR

	[CustomEditor(typeof(PS_ColorChanger))]
	public class PS_ColorChangerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Change Color"))
				((PS_ColorChanger)target).ChangeColor();
			if (GUILayout.Button("Swap \"Current\" with \"New\" colors"))
				((PS_ColorChanger)target).SwapCurrentWithNewColors();
		}
	}

#endif
}