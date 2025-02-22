﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.LinkerGenerator
{
	public sealed class LinkerSettings : ScriptableObject
	{
		private readonly static string SettingsPath = "Assets/LinkerSettings.asset";

		[SerializeField] private List<string> _assembliesToIgnore;
		[SerializeField] private List<string> _assemblyPatternsToIgnore;

		[SerializeField]
		[Tooltip(
			"Relative path from the Assets folder."
		)]
		private string _folderPath;

		[SerializeField] private bool _addDlls;
		[SerializeField] private bool _addAsmdefs;
		[SerializeField] private bool _addRsps;

		public IReadOnlyCollection<string> AssembliesToIgnore => _assembliesToIgnore;
		public IReadOnlyCollection<string> AssemblyPatternsToIgnore => _assemblyPatternsToIgnore;
		public string FolderPath => _folderPath;
		public bool AddDlls => _addDlls;
		public bool AddAsmdefs => _addAsmdefs;
		public bool AddRsps => _addRsps;

		public static LinkerSettings GetOrCreateSettings()
		{
			var settings = AssetDatabase.LoadAssetAtPath<LinkerSettings>(
				SettingsPath
			);
			if (settings != null) return settings;

			settings = CreateInstance<LinkerSettings>();

			settings._assembliesToIgnore = new List<string>();
			settings._folderPath = string.Empty;
			settings._addDlls = true;
			settings._addAsmdefs = false;

			AssetDatabase.CreateAsset(
				settings,
				SettingsPath
			);
			AssetDatabase.SaveAssets();

			return settings;
		}

		public static SerializedObject GetSerializedSettings() => new(
			GetOrCreateSettings()
		);

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider() =>
			new(
				"Project/LinkerSettings",
				SettingsScope.Project
			)
			{
				label = "Linker",
				guiHandler = searchContext =>
				{
					SerializedObject settings = GetSerializedSettings();

					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_assembliesToIgnore)
						),
						new GUIContent(
							"Assemblies to ignore"
						)
					);
					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_assemblyPatternsToIgnore)
						),
						new GUIContent(
							"Assemblies to ignore (Regex)"
						)
					);
					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_folderPath)
						),
						new GUIContent(
							"Link.xml folder path"
						)
					);
					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_addDlls)
						),
						new GUIContent(
							"Scan for DLLs"
						)
					);
					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_addAsmdefs)
						),
						new GUIContent(
							"Scan for ASMDEF files"
						)
					);
					EditorGUILayout.PropertyField(
						settings.FindProperty(
							nameof(_addRsps)
						),
						new GUIContent(
							"Scan for Rsp files"
						)
					);

					settings.ApplyModifiedProperties();
				},
				keywords = new HashSet<string>(
					new[]
					{
						"Dll",
						"Linker"
					}
				)
			};
	}
}