﻿using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Sources.Utils.AssetPathAttribute.Editor
{
#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(SceneReference))]
	public class SceneReferenceDrawer : AssetPathDrawer
	{
		private SerializedProperty m_BuildIndex;
		private SerializedProperty m_Name;

		protected override SerializedProperty GetProperty(SerializedProperty rootProperty)
		{
			m_Name = rootProperty.FindPropertyRelative("m_Name");
			m_BuildIndex = rootProperty.FindPropertyRelative("m_BuildIndex");
			return rootProperty.FindPropertyRelative("m_Path");
		}

		protected override Type ObjectType() => typeof(SceneAsset);

		protected override void OnSelectionMade(Object newSelection, SerializedProperty property)
		{
			if (newSelection == null)
			{
				m_Name.stringValue = "";
				m_BuildIndex.intValue = -1;
			}
			else
			{
				string assetPath = AssetDatabase.GetAssetPath(newSelection);
				UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneByPath(assetPath);
				m_Name.stringValue = scene.name;
				m_BuildIndex.intValue = scene.buildIndex;
			}

			base.OnSelectionMade(newSelection, property);
		}
	}
#endif
}