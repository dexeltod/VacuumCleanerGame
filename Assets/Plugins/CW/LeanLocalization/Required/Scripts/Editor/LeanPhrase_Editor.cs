using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
namespace Plugins.CW.LeanLocalization.Required.Scripts.Editor
{
	using TARGET = LeanPhrase;

	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(TARGET)
	)]
	public class LeanPhrase_Editor : CwEditor
	{
		private readonly static List<string> languageNames = new();

		private readonly static List<LeanPhrase.Entry> entries = new();

		protected override void OnInspector()
		{
			TARGET tgt;
			TARGET[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			entries.Clear();
			entries.AddRange(
				tgt.Entries
			);

			languageNames.Clear();
			languageNames.AddRange(
				LeanLocalization.CurrentLanguages.Keys
			);

			tgt.Data = (LeanPhrase.DataType)GUILayout.Toolbar(
				(int)tgt.Data,
				new[]
				{
					"Text",
					"Object",
					"Sprite"
				}
			);

			Separator();

			foreach (string languageName in languageNames)
			{
				var entry = default(LeanPhrase.Entry);

				if (tgt.TryFindTranslation(
					    languageName,
					    ref entry
				    ))
				{
					DrawEntry(
						tgt,
						entry,
						false
					);

					entries.Remove(
						entry
					);
				}
				else
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(
						languageName,
						EditorStyles.boldLabel
					);

					if (GUILayout.Button(
						    "Add",
						    EditorStyles.miniButton,
						    GUILayout.Width(
							    45.0f
						    )
					    ))
					{
						Undo.RecordObject(
							tgt,
							"Add Translation"
						);

						tgt.AddEntry(
							languageName
						);

						DirtyAndUpdate();
					}

					EditorGUILayout.EndHorizontal();
				}

				Separator();
			}

			if (entries.Count > 0)
				foreach (LeanPhrase.Entry entry in entries)
					DrawEntry(
						tgt,
						entry,
						true
					);
		}

		private void DrawEntry(TARGET tgt, LeanPhrase.Entry entry, bool unexpected)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(
				entry.Language,
				EditorStyles.boldLabel
			);

			if (GUILayout.Button(
				    "Modify",
				    EditorStyles.miniButton,
				    GUILayout.Width(
					    65.0f
				    )
			    ))
			{
				var menu = new GenericMenu();

				foreach (LeanPhrase.Entry otherEntry in tgt.Entries)
				{
					var title = new GUIContent(
						"Auto Translate/From " + otherEntry.Language
					);

					if (entry != otherEntry &&
					    string.IsNullOrEmpty(
						    otherEntry.Text
					    ) ==
					    false)
					{
						string textInput = otherEntry.Text;
						var languageInput = default(LeanLanguage);
						var languageOutput = default(LeanLanguage);

						if (LeanLocalization.CurrentLanguages.TryGetValue(
							    otherEntry.Language,
							    out languageInput
						    ) &&
						    LeanLocalization.CurrentLanguages.TryGetValue(
							    entry.Language,
							    out languageOutput
						    ))
						{
							string languageCodeInput = languageInput.TranslationCode;
							string languageCodeOutput = languageOutput.TranslationCode;

							menu.AddItem(
								title,
								false,
								() =>
								{
									var textOutput = default(string);

									if (TryAutoTranslate(
										    languageCodeInput,
										    languageCodeOutput,
										    textInput,
										    ref textOutput
									    ))
									{
										Undo.RecordObject(
											tgt,
											"Auto Translate"
										);

										entry.Text = textOutput;

										EditorUtility.SetDirty(
											tgt
										);
									}
									else
									{
										Debug.LogError(
											"Failed to auto translate text for some reason."
										);
									}
								}
							);
						}
						else
						{
							menu.AddDisabledItem(
								title,
								false
							);
						}
					}
					else
					{
						menu.AddDisabledItem(
							title,
							false
						);
					}
				}

				menu.AddSeparator(
					""
				);

				menu.AddItem(
					new GUIContent(
						"Remove"
					),
					false,
					() =>
					{
						Undo.RecordObject(
							tgt,
							"Remove Translation"
						);

						tgt.RemoveTranslation(
							entry.Language
						);

						DirtyAndUpdate();
					}
				);

				menu.ShowAsContext();
			}

			EditorGUILayout.EndHorizontal();

			if (unexpected)
				EditorGUILayout.HelpBox(
					"Your LeanLocalization component doesn't define the " + entry.Language + " language.",
					MessageType.Warning
				);

			Undo.RecordObject(
				tgt,
				"Modified Translation"
			);

			EditorGUI.BeginChangeCheck();

			switch (tgt.Data)
			{
				case LeanPhrase.DataType.Text:
					entry.Text = EditorGUILayout.TextArea(
						entry.Text ?? "",
						GUILayout.MinHeight(
							40.0f
						)
					);
					break;
				case LeanPhrase.DataType.Object:
					entry.Object = EditorGUILayout.ObjectField(
						entry.Object,
						typeof(Object),
						true
					);
					break;
				case LeanPhrase.DataType.Sprite:
					entry.Object = EditorGUILayout.ObjectField(
						entry.Object,
						typeof(Sprite),
						true
					);
					break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				DirtyAndUpdate();
				LeanLocalization.UpdateTranslations();
			}

			Separator();
		}

		[MenuItem(
			"Assets/Create/Lean/Localization/Lean Phrase"
		)]
		private static void CreatePhrase()
		{
			CwHelper.CreatePrefabAsset(
				"New Phrase"
			).AddComponent<LeanPhrase>();
		}

		private static bool TryAutoTranslate(string languageCodeInput,
			string languageCodeOutput,
			string wordInput,
			ref string wordOutput)
		{
			try
			{
				string url = string.Format(
					"https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
					languageCodeInput,
					languageCodeOutput,
					HttpUtility.UrlEncode(
						wordInput
					)
				);
				var webClient = new WebClient { Encoding = Encoding.UTF8 };
				string result = webClient.DownloadString(
					url
				);

				wordOutput = result.Substring(
					4,
					result.IndexOf(
						"\",\"",
						4,
						StringComparison.Ordinal
					) -
					4
				);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
#endif