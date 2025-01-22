using System;
using UnityEngine;

namespace Sources.Utils.AssetPathAttribute
{
	[Serializable]
	public struct SceneReference
	{
		[SerializeField] private string m_Name;
		[SerializeField] private string m_Path;
		[SerializeField] private int m_BuildIndex;

		/// <summary>
		///     The name of the scene asset itself.
		/// </summary>
		public string name
		{
			get => m_Name;
			private set => m_Name = value;
		}

		/// <summary>
		///     The asset path to the scene.
		/// </summary>
		public string path
		{
			get => m_Path;
			private set => m_Path = value;
		}

		/// <summary>
		///     The index of the scene in build settings
		/// </summary>
		public int buildIndex
		{
			get => m_BuildIndex;
			private set => m_BuildIndex = value;
		}

		/// <summary>
		///     Returns back if the scene is in the build or not.
		/// </summary>
		public bool isInBuild => buildIndex >= 0;
	}
}