﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>
	///     This class stores a reference to an object (e.g. folder) in your project that contains LeanSource components
	///     so they can be registered.
	/// </summary>
	[Serializable]
	public class LeanPrefab
	{
		private static List<LeanSource> tempSources = new();
		public Object Root;

		[SerializeField] private List<LeanSource> sources;

		[NonSerialized] private int buildingCount;

		[NonSerialized] private bool buildingModified;

		public List<LeanSource> Sources
		{
			get
			{
				if (sources == null) sources = new List<LeanSource>();

				return sources;
			}
		}

		public bool RebuildSources()
		{
			if (sources == null) sources = new List<LeanSource>();

			if (Root != null)
			{
				if (Root is LeanSource)
				{
					buildingCount = 0;
					buildingModified = false;

					AddSource(
						(LeanSource)Root
					);

					return FinalizeBuild();
				}

				if (Root is GameObject)
				{
					buildingCount = 0;
					buildingModified = false;

					FindFromGameObject(
						((GameObject)Root).transform
					);

					return FinalizeBuild();
				}
#if UNITY_EDITOR

				// Folder
				string rootPath = AssetDatabase.GetAssetPath(
					Root
				);

				if (string.IsNullOrEmpty(
					    rootPath
				    ) ==
				    false)
				{
					buildingCount = 0;
					buildingModified = false;

					string basePath = Application.dataPath;
					var baseTail = "Assets";

					if (basePath.EndsWith(
						    baseTail
					    ))
						basePath = basePath.Substring(
							0,
							basePath.Length - baseTail.Length
						);

					FindFromFolder(
						basePath,
						rootPath
					);

					return FinalizeBuild();
				}
#endif
			}

			return false;
		}

		private bool FinalizeBuild()
		{
			for (int i = sources.Count - 1; i >= buildingCount; i--)
			{
				sources.RemoveAt(
					i
				);
				buildingModified = true;
			}

			return buildingModified;
		}

		private void AddSource(LeanSource source)
		{
			if (buildingCount < sources.Count)
			{
				if (sources[buildingCount] != source)
				{
					sources[buildingCount] = source;

					buildingModified = true;
				}
			}
			else
			{
				sources.Add(
					source
				);

				buildingModified = true;
			}

			buildingCount++;
		}

		private void FindFromGameObject(Transform prefab)
		{
			prefab.GetComponents(
				tempSources
			);

			if (tempSources.Count > 0)
				for (var i = 0; i < tempSources.Count; i++)
					AddSource(
						tempSources[i]
					);
			else
				for (var i = 0; i < prefab.childCount; i++)
					FindFromGameObject(
						prefab.GetChild(
							i
						)
					);
		}
#if UNITY_EDITOR
		private void FindFromFolder(string basePath, string rootPath)
		{
			string fullPath = basePath + rootPath;

			if (Directory.Exists(
				    fullPath
			    ))
			{
				string[] subFolders = Directory.GetDirectories(
					fullPath
				);

				for (var i = 0; i < subFolders.Length; i++)
					FindFromFolder(
						basePath,
						subFolders[i].Substring(
							basePath.Length
						)
					);

				string[] subAssets = Directory.GetFiles(
					fullPath,
					"*.prefab"
				);

				for (var i = 0; i < subAssets.Length; i++)
					FindFromFolder(
						basePath,
						subAssets[i].Substring(
							basePath.Length
						)
					);
			}
			// File
			else
			{
				var subGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(
					rootPath
				);

				if (subGameObject != null)
					FindFromGameObject(
						subGameObject.transform
					);
			}
		}
#endif
	}
}