using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>This attribute can be added to any int field to make it a random seed value that can easily be randomized.</summary>
	public class CwSeedAttribute : PropertyAttribute
	{
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(
		typeof(CwSeedAttribute)
	)]
	public class CwSeedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var rect1 = position;
			rect1.xMax = position.xMax - 20;
			var rect2 = position;
			rect2.xMin = position.xMax - 18;

			EditorGUI.PropertyField(
				rect1,
				property,
				label
			);

			if (GUI.Button(
				    rect2,
				    "R"
			    ) ==
			    true)
			{
				property.intValue = Random.Range(
					int.MinValue,
					int.MaxValue
				);
			}
		}
	}

#endif
}