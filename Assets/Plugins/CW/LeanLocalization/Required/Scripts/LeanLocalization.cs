using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>
	///     This component manages a global list of translations for easy access.
	///     Translations are gathered from the <b>prefabs</b> list, as well as from any active and enabled <b>LeanSource</b>
	///     components in the scene.
	/// </summary>
	[ExecuteInEditMode]
	[HelpURL(
		HelpUrlPrefix + "LeanLocalization"
	)]
	[AddComponentMenu(
		ComponentPathPrefix + "Localization"
	)]
	public class LeanLocalization : MonoBehaviour
	{
		public enum DetectType
		{
			None,
			SystemLanguage,
			CurrentCulture,
			CurrentUICulture
		}

		public enum SaveLoadType
		{
			None,
			WhenChanged,
			WhenChangedAlt
		}

		public const string HelpUrlPrefix = LeanCommon.Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanLocalization#";

		public const string ComponentPathPrefix = "Lean/Localization/Lean ";

		/// <summary>All active and enabled LeanLocalization components.</summary>
		public static List<LeanLocalization> Instances = new();

		public static Dictionary<string, LeanToken> CurrentTokens = new();

		public static Dictionary<string, LeanLanguage> CurrentLanguages = new();

		public static Dictionary<string, string> CurrentAliases = new();

		/// <summary>Dictionary of all the phrase names mapped to their current translations.</summary>
		public static Dictionary<string, LeanTranslation> CurrentTranslations = new();

		private static bool pendingUpdates;

		private readonly static Dictionary<string, LeanTranslation> tempTranslations = new();

		private readonly static List<LeanSource> tempSources = new(
			1024
		);

		/// <summary>The language that is currently being used by this instance.</summary>
		[LeanLanguageName]
		[SerializeField]
		private string currentLanguage;

		[SerializeField] private DetectType detectLanguage = DetectType.SystemLanguage;

		[SerializeField] [LeanLanguageName] private string defaultLanguage;

		[SerializeField] private SaveLoadType saveLoad = SaveLoadType.WhenChanged;

		[SerializeField] private List<LeanPrefab> prefabs;

		/// <summary>How should the cultures be used to detect the user's device language?</summary>
		public DetectType DetectLanguage
		{
			set => detectLanguage = value;
			get => detectLanguage;
		}

		/// <summary>If the application is started and no language has been loaded or auto detected, this language will be used.</summary>
		public string DefaultLanguage
		{
			set => defaultLanguage = value;
			get => defaultLanguage;
		}

		/// <summary>
		///     This allows you to control if/how this component's <b>CurrentLanguage</b> setting should save/load.
		///     None = Only the <b>DetectLanguage</b> and <b>DefaultLanguage</b> settings will be used.
		///     WhenChanged = If the <b>CurrentLanguage</b> gets manually changed, automatically save/load it to PlayerPrefs?
		///     NOTE: This save data can be cleared with <b>ClearSave</b> context menu option.
		/// </summary>
		public SaveLoadType SaveLoad
		{
			set => saveLoad = value;
			get => saveLoad;
		}

		/// <summary>This stores all prefabs and folders managed by this LeanLocalization instance.</summary>
		public List<LeanPrefab> Prefabs
		{
			get
			{
				if (prefabs == null) prefabs = new List<LeanPrefab>();
				return prefabs;
			}
		}

		/// <summary>Change the current language of this instance?</summary>
		public string CurrentLanguage
		{
			set
			{
				if (currentLanguage != value)
				{
					currentLanguage = value;

					if (saveLoad != SaveLoadType.None) SaveNow();

					UpdateTranslations();
				}
			}

			get => currentLanguage;
		}

		protected virtual void Update()
		{
			UpdateTranslations(
				false
			);
		}

		/// <summary>Set the instance, merge old instance, and update translations.</summary>
		protected virtual void OnEnable()
		{
			Instances.Add(
				this
			);

			UpdateTranslations();
		}

		/// <summary>Unset instance?</summary>
		protected virtual void OnDisable()
		{
			Instances.Remove(
				this
			);

			UpdateTranslations();
		}

#if UNITY_EDITOR
		// Inspector modified?
		protected virtual void OnValidate()
		{
			UpdateTranslations();
		}
#endif

		/// <summary>Called when the language or translations change.</summary>
		public static event Action OnLocalizationChanged;

		/// <summary>When rebuilding translations this method is called from any <b>LeanSource</b> components that define a token.</summary>
		public static void RegisterToken(string name, LeanToken token)
		{
			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false &&
			    token != null &&
			    CurrentTokens.ContainsKey(
				    name
			    ) ==
			    false)
				CurrentTokens.Add(
					name,
					token
				);
		}

		/// <summary>
		///     When rebuilding translations this method is called from any <b>LeanSource</b> components that define a
		///     transition.
		/// </summary>
		public static LeanTranslation RegisterTranslation(string name)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false &&
			    CurrentTranslations.TryGetValue(
				    name,
				    out translation
			    ) ==
			    false)
			{
				if (tempTranslations.TryGetValue(
					    name,
					    out translation
				    ))
				{
					tempTranslations.Remove(
						name
					);

					CurrentTranslations.Add(
						name,
						translation
					);
				}
				else
				{
					translation = new LeanTranslation(
						name
					);

					CurrentTranslations.Add(
						name,
						translation
					);
				}
			}

			return translation;
		}

		[ContextMenu(
			"Clear Save"
		)]
		public void ClearSave()
		{
			PlayerPrefs.DeleteKey(
				"LeanLocalization.CurrentLanguage"
			);

			PlayerPrefs.Save();
		}

		[ContextMenu(
			"Clear Save Alt"
		)]
		public void ClearSaveAlt()
		{
			PlayerPrefs.DeleteKey(
				"LeanLocalization.CurrentLanguageAlt"
			);

			PlayerPrefs.Save();
		}

		private void SaveNow()
		{
			if (saveLoad == SaveLoadType.WhenChanged)
				PlayerPrefs.SetString(
					"LeanLocalization.CurrentLanguage",
					currentLanguage
				);
			else if (saveLoad == SaveLoadType.WhenChangedAlt)
				PlayerPrefs.SetString(
					"LeanLocalization.CurrentLanguageAlt",
					currentLanguage
				);

			PlayerPrefs.Save();
		}

		private void LoadNow()
		{
			if (saveLoad == SaveLoadType.WhenChanged)
				currentLanguage = PlayerPrefs.GetString(
					"LeanLocalization.CurrentLanguage"
				);
			else if (saveLoad == SaveLoadType.WhenChangedAlt)
				currentLanguage = PlayerPrefs.GetString(
					"LeanLocalization.CurrentLanguageAlt"
				);
		}

		/// <summary>This sets the current language using the specified language name.</summary>
		public void SetCurrentLanguage(string newLanguage)
		{
			CurrentLanguage = newLanguage;
		}

		/// <summary>This sets the current language of all instances using the specified language name.</summary>
		public static void SetCurrentLanguageAll(string newLanguage)
		{
			foreach (LeanLocalization instance in Instances) instance.CurrentLanguage = newLanguage;
		}

		/// <summary>
		///     This returns the <b>CurrentLanguage</b> value from the first <b>LeanLocalization</b> instance in the scene if
		///     it exists, or null.
		/// </summary>
		public static string GetFirstCurrentLanguage()
		{
			if (Instances.Count > 0) return Instances[0].CurrentLanguage;

			return null;
		}

		public static LeanLocalization GetOrCreateInstance()
		{
			if (Instances.Count == 0)
				new GameObject(
					"LeanLocalization"
				).AddComponent<LeanLocalization>();

			return Instances[0];
		}

		/// <summary>
		///     This adds the specified UnityEngine.Object to this LeanLocalization instance, allowing it to be registered as
		///     a prefab.
		/// </summary>
		public void AddPrefab(Object root)
		{
			for (int i = Prefabs.Count - 1; i >= 0; i--) // NOTE: Property
				if (prefabs[i].Root == root)
					return;

			var prefab = new LeanPrefab();

			prefab.Root = root;

			prefabs.Add(
				prefab
			);
		}

		/// <summary>This calls <b>AddLanguage</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanLanguage AddLanguageToFirst(string name) =>
			GetOrCreateInstance().AddLanguage(
				name
			);

		/// <summary>This creates a new token with the specified name, and adds it to the current GameObject.</summary>
		public LeanLanguage AddLanguage(string name)
		{
			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
			{
				var root = new GameObject(
					name
				);
				var language = root.AddComponent<LeanLanguage>();

				root.transform.SetParent(
					transform,
					false
				);

				return language;
			}

			return null;
		}

		/// <summary>This calls <b>AddToken</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanToken AddTokenToFirst(string name) =>
			GetOrCreateInstance().AddToken(
				name
			);

		/// <summary>This creates a new token with the specified name, and adds it to the current GameObject.</summary>
		public LeanToken AddToken(string name)
		{
			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
			{
				var root = new GameObject(
					name
				);
				var token = root.AddComponent<LeanToken>();

				root.transform.SetParent(
					transform,
					false
				);

				return token;
			}

			return null;
		}

		/// <summary>
		///     This allows you to set the value of the token with the specified name.
		///     If no token exists and allowCreation is enabled, then one will be created for you.
		/// </summary>
		public static void SetToken(string name, string value, bool allowCreation = true)
		{
			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
			{
				var token = default(LeanToken);

				if (CurrentTokens.TryGetValue(
					    name,
					    out token
				    ))
				{
					token.Value = value;
				}
				else if (allowCreation)
				{
					token = AddTokenToFirst(
						name
					);

					token.Value = value;
				}
			}
		}

		/// <summary>
		///     This allows you to get the value of the token with the specified name.
		///     If no token exists, then the defaultValue will be returned.
		/// </summary>
		public static string GetToken(string name, string defaultValue = null)
		{
			var token = default(LeanToken);

			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
				if (CurrentTokens.TryGetValue(
					    name,
					    out token
				    ))
					return token.Value;

			return defaultValue;
		}

		/// <summary>This calls <b>AddPhrase</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanPhrase AddPhraseToFirst(string name) =>
			GetOrCreateInstance().AddPhrase(
				name
			);

		/// <summary>This creates a new phrase with the specified name, and adds it to the current GameObject.</summary>
		public LeanPhrase AddPhrase(string name)
		{
			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
			{
				var root = new GameObject(
					name
				);
				var phrase = root.AddComponent<LeanPhrase>();

				root.transform.SetParent(
					transform,
					false
				);

				return phrase;
			}

			return null;
		}

		/// <summary>This will return the translation with the specified name, or null if none was found.</summary>
		public static LeanTranslation GetTranslation(string name)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false)
				CurrentTranslations.TryGetValue(
					name,
					out translation
				);

			return translation;
		}

		/// <summary>This will return the translated string with the specified name, or the fallback if none is found.</summary>
		public static string GetTranslationText(string name, string fallback = null, bool replaceTokens = true)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false &&
			    CurrentTranslations.TryGetValue(
				    name,
				    out translation
			    ) &&
			    translation.Data is string)
				fallback = (string)translation.Data;

			if (replaceTokens)
				fallback = LeanTranslation.FormatText(
					fallback
				);

			return fallback;
		}

		/// <summary>This will return the translated UnityEngine.Object with the specified name, or the fallback if none is found.</summary>
		public static T GetTranslationObject<T>(string name, T fallback = null)
			where T : Object
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(
				    name
			    ) ==
			    false &&
			    CurrentTranslations.TryGetValue(
				    name,
				    out translation
			    ) &&
			    translation.Data is T)
				return (T)translation.Data;

			return fallback;
		}

		/// <summary>This rebuilds the dictionary used to quickly map phrase names to translations for the current language.</summary>
		public static void UpdateTranslations(bool forceUpdate = true)
		{
			if (pendingUpdates || forceUpdate)
			{
				pendingUpdates = false;

				// Copy previous translations to temp dictionary
				tempTranslations.Clear();

				foreach (KeyValuePair<string, LeanTranslation> pair in CurrentTranslations)
				{
					LeanTranslation translation = pair.Value;

					translation.Clear();

					tempTranslations.Add(
						pair.Key,
						translation
					);
				}

				// Clear currents
				CurrentTokens.Clear();
				CurrentLanguages.Clear();
				CurrentAliases.Clear();
				CurrentTranslations.Clear();

				// Rebuild all currents
				foreach (LeanLocalization instance in Instances) instance.RegisterAll();

				// Notify changes?
				if (OnLocalizationChanged != null) OnLocalizationChanged();
			}
		}

		/// <summary>If you call this method, then UpdateTranslations will be called next Update.</summary>
		public static void DelayUpdateTranslations()
		{
			pendingUpdates = true;

#if UNITY_EDITOR
			// Go through all enabled phrases
			for (var i = 0; i < Instances.Count; i++)
			{
				//	UnityEditor.EditorUtility.SetDirty(Instances[i].gameObject);
			}
#endif
		}

		private void RegisterAll()
		{
			GetComponentsInChildren(
				tempSources
			);

			// First pass
			if (prefabs != null)
				foreach (LeanPrefab prefab in prefabs)
				foreach (LeanSource source in prefab.Sources)
					source.Register();

			foreach (LeanSource source in tempSources) source.Register();

			// Update language (depends on first pass)
			UpdateCurrentLanguage();

			// Second pass
			if (prefabs != null)
				foreach (LeanPrefab prefab in prefabs)
				foreach (LeanSource source in prefab.Sources)
					source.Register(
						currentLanguage,
						defaultLanguage
					);

			foreach (LeanSource source in tempSources)
				source.Register(
					currentLanguage,
					defaultLanguage
				);

			tempSources.Clear();
		}

		private void UpdateCurrentLanguage()
		{
			// Load saved language?
			if (saveLoad != SaveLoadType.None) LoadNow();

			// Find language by culture?
			if (string.IsNullOrEmpty(
				    currentLanguage
			    ))
				switch (detectLanguage)
				{
					case DetectType.SystemLanguage:
					{
						CurrentAliases.TryGetValue(
							Application.systemLanguage.ToString(),
							out currentLanguage
						);
					}
						break;

					case DetectType.CurrentCulture:
					{
						CultureInfo cultureInfo = CultureInfo.CurrentCulture;

						if (cultureInfo != null)
							CurrentAliases.TryGetValue(
								cultureInfo.Name,
								out currentLanguage
							);
					}
						break;

					case DetectType.CurrentUICulture:
					{
						CultureInfo cultureInfo = CultureInfo.CurrentUICulture;

						if (cultureInfo != null)
							CurrentAliases.TryGetValue(
								cultureInfo.Name,
								out currentLanguage
							);
					}
						break;
				}

			// Use default language?
			if (string.IsNullOrEmpty(
				    currentLanguage
			    ))
				currentLanguage = defaultLanguage;
		}

#if UNITY_EDITOR
		/// <summary>This exports all text phrases in the LeanLocalization component for the Language specified by this component.</summary>
		[ContextMenu(
			"Export CurrentLanguage To CSV (Comma Format)"
		)]
		private void ExportTextAsset()
		{
			if (string.IsNullOrEmpty(
				    currentLanguage
			    ) ==
			    false)
			{
				// Find where we want to save the file
				string path = EditorUtility.SaveFilePanelInProject(
					"Export Text Asset for " + currentLanguage,
					currentLanguage,
					"csv",
					""
				);

				// Make sure we didn't cancel the panel
				if (string.IsNullOrEmpty(
					    path
				    ) ==
				    false)
					DoExportTextAsset(
						path
					);
			}
		}

		private void DoExportTextAsset(string path)
		{
			var data = "";
			var gaps = false;

			// Add all phrase names and existing translations to lines
			foreach (KeyValuePair<string, LeanTranslation> pair in CurrentTranslations)
			{
				LeanTranslation translation = pair.Value;

				if (gaps) data += Environment.NewLine;

				data += pair.Key + ",\"";
				gaps = true;

				if (translation.Data is string)
				{
					var text = (string)translation.Data;

					// Replace all new line permutations with the new line token
					text = text.Replace(
						"\r\n",
						"\n"
					);
					text = text.Replace(
						"\n\r",
						"\n"
					);
					text = text.Replace(
						"\r",
						"\n"
					);

					data += text;
				}

				data += "\"";
			}

			// Write text to file
			using (FileStream file = File.OpenWrite(
				       path
			       ))
			{
				var encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(
					data
				);

				file.Write(
					bytes,
					0,
					bytes.Length
				);
			}

			// Import asset into project
			AssetDatabase.ImportAsset(
				path
			);

			// Replace Source with new Text Asset?
			var textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(
				path,
				typeof(TextAsset)
			);

			if (textAsset != null)
			{
				EditorGUIUtility.PingObject(
					textAsset
				);

				EditorUtility.SetDirty(
					this
				);
			}
		}
#endif
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanLocalization)
	)]
	public class LeanLocalization_Editor : CwEditor
	{
		private readonly static List<PresetLanguage> presetLanguages = new();

		private static string translationFilter;

		private readonly static List<string> missing = new();

		private readonly static List<string> clashes = new();

		private static string languagesFilter;

		private static string tokensFilter;

		private int expandPrefab = -1;

		private LeanTranslation expandTranslation;

		protected override void OnInspector()
		{
			LeanLocalization tgt;
			LeanLocalization[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			LeanLocalization.UpdateTranslations();

			if (Draw(
				    "currentLanguage",
				    "The language that is currently being used by this instance."
			    ))
				Each(
					tgts,
					t => t.CurrentLanguage = serializedObject.FindProperty(
						"currentLanguage"
					).stringValue,
					true
				);

			Draw(
				"saveLoad",
				"This allows you to control if/how this component's <b>CurrentLanguage</b> setting should save/load.\n\nNone = Only the <b>DetectLanguage</b> and <b>DefaultLanguage</b> settings will be used.\n\nWhenChanged = If the <b>CurrentLanguage</b> gets manually changed, automatically save/load it to PlayerPrefs?\n\nNOTE: This save data can be cleared with <b>ClearSave</b> context menu option."
			);

			Separator();

			Draw(
				"detectLanguage",
				"How should the cultures be used to detect the user's device language?"
			);
			BeginDisabled();
			BeginIndent();

			switch (tgt.DetectLanguage)
			{
				case LeanLocalization.DetectType.SystemLanguage:
					EditorGUILayout.TextField(
						"SystemLanguage",
						Application.systemLanguage.ToString()
					);
					break;
				case LeanLocalization.DetectType.CurrentCulture:
					EditorGUILayout.TextField(
						"CurrentCulture",
						CultureInfo.CurrentCulture.ToString()
					);
					break;
				case LeanLocalization.DetectType.CurrentUICulture:
					EditorGUILayout.TextField(
						"CurrentUICulture",
						CultureInfo.CurrentUICulture.ToString()
					);
					break;
			}

			EndIndent();
			EndDisabled();
			Draw(
				"defaultLanguage",
				"If the application is started and no language has been loaded or auto detected, this language will be used."
			);

			Separator();

			DrawPrefabs(
				tgt
			);

			Separator();

			DrawLanguages();

			Separator();

			DrawTokens();

			Separator();

			DrawTranslations();
		}

		private void DrawPrefabs(LeanLocalization tgt)
		{
			Rect rectA = Reserve();
			Rect rectB = rectA;
			rectB.xMin += EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(
				rectA,
				"Prefabs",
				EditorStyles.boldLabel
			);
			Object newPrefab = EditorGUI.ObjectField(
				rectB,
				"",
				default,
				typeof(Object),
				false
			);

			if (newPrefab != null)
			{
				Undo.RecordObject(
					tgt,
					"Add Source"
				);

				tgt.AddPrefab(
					newPrefab
				);

				DirtyAndUpdate();
			}

			BeginIndent();

			for (var i = 0; i < tgt.Prefabs.Count; i++)
				DrawPrefabs(
					tgt,
					i
				);

			EndIndent();
		}

		private void DrawPrefabs(LeanLocalization tgt, int index)
		{
			Rect rectA = Reserve();
			Rect rectB = rectA;
			rectB.xMax -= 22.0f;
			Rect rectC = rectA;
			rectC.xMin = rectC.xMax - 20.0f;
			LeanPrefab prefab = tgt.Prefabs[index];
			var rebuilt = false;
			bool expand = EditorGUI.Foldout(
				new Rect(
					rectA.x,
					rectA.y,
					20,
					rectA.height
				),
				expandPrefab == index,
				""
			);

			if (expand)
				expandPrefab = index;
			else if (expandPrefab == index) expandPrefab = -1;

			BeginDisabled();
			BeginError(
				prefab.Root == null
			);
			EditorGUI.ObjectField(
				rectB,
				prefab.Root,
				typeof(Object),
				false
			);
			EndError();

			if (prefab.Root != null)
			{
				Undo.RecordObject(
					tgt,
					"Rebuild Sources"
				);

				rebuilt |= prefab.RebuildSources();

				if (expand)
				{
					List<LeanSource> sources = prefab.Sources;

					BeginIndent();

					foreach (LeanSource source in sources)
						EditorGUI.ObjectField(
							Reserve(),
							source,
							typeof(LeanSource),
							false
						);

					EndIndent();
				}
			}

			EndDisabled();

			if (rebuilt) DirtyAndUpdate();

			if (GUI.Button(
				    rectC,
				    "X",
				    EditorStyles.miniButton
			    ))
			{
				Undo.RecordObject(
					tgt,
					"Remove Prefab"
				);

				tgt.Prefabs.RemoveAt(
					index
				);

				DirtyAndUpdate();

				if (expand) expandPrefab = -1;
			}
		}

		private void DrawTranslations()
		{
			Rect rectA = Reserve();
			Rect rectB = rectA;
			rectB.xMin += EditorGUIUtility.labelWidth;
			rectB.xMax -= 37.0f;
			Rect rectC = rectA;
			rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(
				rectA,
				"Translations",
				EditorStyles.boldLabel
			);
			translationFilter = EditorGUI.TextField(
				rectB,
				"",
				translationFilter
			);
			BeginDisabled(
				string.IsNullOrEmpty(
					translationFilter
				) ||
				LeanLocalization.CurrentTranslations.ContainsKey(
					translationFilter
				)
			);

			if (GUI.Button(
				    rectC,
				    "Add",
				    EditorStyles.miniButton
			    ))
			{
				LeanPhrase phrase = LeanLocalization.AddPhraseToFirst(
					translationFilter
				);

				LeanLocalization.UpdateTranslations();

				Selection.activeObject = phrase;

				EditorGUIUtility.PingObject(
					phrase
				);
			}

			EndDisabled();

			if (LeanLocalization.CurrentTranslations.Count == 0 &&
			    string.IsNullOrEmpty(
				    translationFilter
			    ))
			{
				Info(
					"Type in the name of a translation, and click the 'Add' button. Or, drag and drop a prefab that contains some."
				);
			}
			else
			{
				var total = 0;

				BeginIndent();

				foreach (KeyValuePair<string, LeanTranslation> pair in LeanLocalization.CurrentTranslations)
				{
					string name = pair.Key;

					if (string.IsNullOrEmpty(
						    translationFilter
					    ) ||
					    name.IndexOf(
						    translationFilter,
						    StringComparison.InvariantCultureIgnoreCase
					    ) >=
					    0)
					{
						LeanTranslation translation = pair.Value;
						Rect rectT = Reserve();
						bool expand = EditorGUI.Foldout(
							new Rect(
								rectT.x,
								rectT.y,
								20,
								rectT.height
							),
							expandTranslation == translation,
							""
						);

						if (expand)
							expandTranslation = translation;
						else if (expandTranslation == translation) expandTranslation = null;

						CalculateTranslation(
							pair.Value
						);

						object data = translation.Data;

						total++;

						BeginDisabled();
						BeginError(
							missing.Count > 0 || clashes.Count > 0
						);

						if (data is Object)
							EditorGUI.ObjectField(
								rectT,
								name,
								(Object)data,
								typeof(Object),
								true
							);
						else
							EditorGUI.TextField(
								rectT,
								name,
								data != null ? data.ToString() : ""
							);

						EndError();

						if (expand)
						{
							BeginIndent();

							foreach (LeanTranslation.Entry entry in translation.Entries)
							{
								BeginError(
									clashes.Contains(
										entry.Language
									)
								);
								EditorGUILayout.ObjectField(
									entry.Language,
									entry.Owner,
									typeof(Object),
									true
								);
								EndError();
							}

							EndIndent();
						}

						EndDisabled();

						if (expand)
						{
							foreach (string language in missing)
								Warning(
									"This translation isn't defined for the " + language + " language."
								);

							foreach (string language in clashes)
								Warning(
									"This translation is defined multiple times for the " +
									language +
									" language."
								);
						}
					}
				}

				EndIndent();

				if (total == 0)
					Info(
						"No translation with this name exists, click the 'Add' button to create it."
					);
			}
		}

		private static void CalculateTranslation(LeanTranslation translation)
		{
			missing.Clear();
			clashes.Clear();

			foreach (string language in LeanLocalization.CurrentLanguages.Keys)
				if (translation.Entries.Exists(
					    e => e.Language == language
				    ) ==
				    false)
					missing.Add(
						language
					);

			foreach (LeanTranslation.Entry entry in translation.Entries)
			{
				string language = entry.Language;

				if (clashes.Contains(
					    language
				    ) ==
				    false)
					if (translation.LanguageCount(
						    language
					    ) >
					    1)
						clashes.Add(
							language
						);
			}
		}

		private void DrawLanguages()
		{
			Rect rectA = Reserve();
			Rect rectB = rectA;
			rectB.xMin += EditorGUIUtility.labelWidth;
			rectB.xMax -= 37.0f;
			Rect rectC = rectA;
			rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(
				rectA,
				"Languages",
				EditorStyles.boldLabel
			);
			languagesFilter = EditorGUI.TextField(
				rectB,
				"",
				languagesFilter
			);

			//BeginDisabled(string.IsNullOrEmpty(languagesFilter) == true || LeanLocalization.CurrentLanguages.ContainsKey(languagesFilter) == true);
			if (GUI.Button(
				    rectC,
				    "Add",
				    EditorStyles.miniButton
			    ))
			{
				if (string.IsNullOrEmpty(
					    languagesFilter
				    ))
				{
					var menu = new GenericMenu();

					IEnumerable<GameObject> languagePrefabs = AssetDatabase.FindAssets(
							"t:GameObject"
						)
						.Select(
							guid =>
								AssetDatabase.LoadAssetAtPath<GameObject>(
									AssetDatabase.GUIDToAssetPath(
										guid
									)
								)
						)
						.Where(
							prefab => prefab.GetComponent<LeanLanguage>() != null
						);

					foreach (GameObject languagePrefab in languagePrefabs)
						if (LeanLocalization.CurrentLanguages.ContainsKey(
							    languagePrefab.name
						    ))
							menu.AddItem(
								new GUIContent(
									languagePrefab.name
								),
								true,
								() => { }
							);
						else
							menu.AddItem(
								new GUIContent(
									languagePrefab.name
								),
								false,
								() =>
								{
									if (LeanLocalization.Instances.Count > 0)
									{
										Object language = PrefabUtility.InstantiatePrefab(
											languagePrefab,
											LeanLocalization.Instances[0].transform
										);

										LeanLocalization.UpdateTranslations();

										Selection.activeObject = language;

										EditorGUIUtility.PingObject(
											language
										);
									}
								}
							);

					menu.ShowAsContext();
				}
				else
				{
					LeanLanguage language = LeanLocalization.AddLanguageToFirst(
						languagesFilter
					);

					LeanLocalization.UpdateTranslations();

					Selection.activeObject = language;

					EditorGUIUtility.PingObject(
						language
					);
				}
			}
			//EndDisabled();

			if (LeanLocalization.CurrentLanguages.Count > 0 ||
			    string.IsNullOrEmpty(
				    languagesFilter
			    ) ==
			    false)
			{
				var total = 0;

				BeginIndent();
				BeginDisabled();

				foreach (KeyValuePair<string, LeanLanguage> pair in LeanLocalization.CurrentLanguages)
					if (string.IsNullOrEmpty(
						    languagesFilter
					    ) ||
					    pair.Key.IndexOf(
						    languagesFilter,
						    StringComparison.InvariantCultureIgnoreCase
					    ) >=
					    0)
					{
						EditorGUILayout.ObjectField(
							pair.Key,
							pair.Value,
							typeof(Object),
							true
						);
						total++;
					}

				EndDisabled();
				EndIndent();

				if (total == 0)
					EditorGUILayout.HelpBox(
						"No language with this name exists, click the 'Add' button to create it.",
						MessageType.Info
					);
			}
		}

		private void DrawTokens()
		{
			Rect rectA = Reserve();
			Rect rectB = rectA;
			rectB.xMin += EditorGUIUtility.labelWidth;
			rectB.xMax -= 37.0f;
			Rect rectC = rectA;
			rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(
				rectA,
				"Tokens",
				EditorStyles.boldLabel
			);
			tokensFilter = EditorGUI.TextField(
				rectB,
				"",
				tokensFilter
			);
			BeginDisabled(
				string.IsNullOrEmpty(
					tokensFilter
				) ||
				LeanLocalization.CurrentTokens.ContainsKey(
					tokensFilter
				)
			);

			if (GUI.Button(
				    rectC,
				    "Add",
				    EditorStyles.miniButton
			    ))
			{
				LeanToken token = LeanLocalization.AddTokenToFirst(
					tokensFilter
				);

				LeanLocalization.UpdateTranslations();

				Selection.activeObject = token;

				EditorGUIUtility.PingObject(
					token
				);
			}

			EndDisabled();

			if (LeanLocalization.CurrentTokens.Count > 0 ||
			    string.IsNullOrEmpty(
				    tokensFilter
			    ) ==
			    false)
			{
				var total = 0;

				BeginIndent();
				BeginDisabled();

				foreach (KeyValuePair<string, LeanToken> pair in LeanLocalization.CurrentTokens)
					if (string.IsNullOrEmpty(
						    tokensFilter
					    ) ||
					    pair.Key.IndexOf(
						    tokensFilter,
						    StringComparison.InvariantCultureIgnoreCase
					    ) >=
					    0)
					{
						EditorGUILayout.ObjectField(
							pair.Key,
							pair.Value,
							typeof(Object),
							true
						);
						total++;
					}

				EndDisabled();
				EndIndent();

				if (total == 0)
					EditorGUILayout.HelpBox(
						"No token with this name exists, click the 'Add' button to create it.",
						MessageType.Info
					);
			}
		}

		private void AddLanguage(LeanLocalization tgt, PresetLanguage presetLanguage)
		{
			Undo.RecordObject(
				tgt,
				"Add Language"
			);

			tgt.AddLanguage(
				presetLanguage.Name
			);

			DirtyAndUpdate();
		}

		private static void AddPresetLanguage(string name, params string[] cultures)
		{
			var presetLanguage = new PresetLanguage();

			presetLanguage.Name = name;
			presetLanguage.Cultures = cultures;

			presetLanguages.Add(
				presetLanguage
			);
		}

		[MenuItem(
			"GameObject/Lean/Localization",
			false,
			1
		)]
		private static void CreateLocalization()
		{
			var gameObject = new GameObject(
				typeof(LeanLocalization).Name
			);

			Undo.RegisterCreatedObjectUndo(
				gameObject,
				"Create LeanLocalization"
			);

			gameObject.AddComponent<LeanLocalization>();

			Selection.activeGameObject = gameObject;
		}

		private class PresetLanguage
		{
			public string[] Cultures;
			public string Name;
		}
	}

#endif
}