using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>This attribute allows you to select a translation from all the localizations in the scene.</summary>
	public class LeanTranslationNameAttribute : PropertyAttribute
	{
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(
		typeof(LeanTranslationNameAttribute)
	)]
	public class LeanTranslationNameDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect left = position;
			left.xMax -= 40;
			Rect right = position;
			right.xMin = left.xMax + 2;
			Color color = GUI.color;
			bool exists = LeanLocalization.CurrentTranslations.ContainsKey(
				property.stringValue
			);

			if (exists == false) GUI.color = Color.red;

			EditorGUI.PropertyField(
				left,
				property
			);

			GUI.color = color;

			if (GUI.Button(
				    right,
				    "List"
			    ))
			{
				var menu = new GenericMenu();

				if (string.IsNullOrEmpty(
					    property.stringValue
				    ) ==
				    false)
				{
					if (exists)
					{
						var translation = default(LeanTranslation);

						if (LeanLocalization.CurrentTranslations.TryGetValue(
							    property.stringValue,
							    out translation
						    ))
							foreach (LeanTranslation.Entry entry in translation.Entries)
							{
								Object owner = entry.Owner;
								menu.AddItem(
									new GUIContent(
										"Select/" + entry.Language
									),
									false,
									() =>
									{
										Selection.activeObject = owner;
										EditorGUIUtility.PingObject(
											owner
										);
									}
								);
							}
					}
					else
					{
						menu.AddItem(
							new GUIContent(
								"Add: " +
								property.stringValue.Replace(
									'/',
									'\\'
								)
							),
							false,
							() =>
							{
								LeanPhrase phrase = LeanLocalization.AddPhraseToFirst(
									property.stringValue
								);
								LeanLocalization.UpdateTranslations();
								Selection.activeObject = phrase;
								EditorGUIUtility.PingObject(
									phrase
								);
							}
						);
					}

					menu.AddItem(
						GUIContent.none,
						false,
						null
					);
				}

				foreach (string translationName in LeanLocalization.CurrentTranslations.Keys)
					menu.AddItem(
						new GUIContent(
							translationName
						),
						property.stringValue == translationName,
						() =>
						{
							property.stringValue = translationName;
							property.serializedObject.ApplyModifiedProperties();
						}
					);

				if (menu.GetItemCount() > 0)
					menu.DropDown(
						right
					);
				else
					Debug.LogWarning(
						"Your scene doesn't contain any phrases, so the phrase name list couldn't be created."
					);
			}
		}
	}

#endif
}