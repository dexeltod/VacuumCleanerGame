using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>
	///     This attribute allows you to modify a normal string field into one that has a dropdown list that allows you to
	///     pick a language.
	/// </summary>
	public class LeanLanguageNameAttribute : PropertyAttribute
	{
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(
		typeof(LeanLanguageNameAttribute)
	)]
	public class LeanLanguageNameDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect rectA = position;
			rectA.xMax -= 37.0f;
			Rect rectB = position;
			rectB.xMin = rectB.xMax - 35.0f;

			EditorGUI.PropertyField(
				rectA,
				property,
				label
			);

			if (GUI.Button(
				    rectB,
				    "List"
			    ))
			{
				var menu = new GenericMenu();

				foreach (string languageName in LeanLocalization.CurrentLanguages.Keys)
					menu.AddItem(
						new GUIContent(
							languageName
						),
						property.stringValue == languageName,
						() =>
						{
							property.stringValue = languageName;
							property.serializedObject.ApplyModifiedProperties();
						}
					);

				if (menu.GetItemCount() == 0)
					menu.AddDisabledItem(
						new GUIContent(
							"Your scene doesn't contain any languages."
						)
					);

				menu.ShowAsContext();
			}
		}
	}

#endif
}