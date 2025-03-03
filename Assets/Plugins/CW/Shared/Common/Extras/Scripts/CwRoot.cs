﻿using System.Collections.Generic;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>
	///     If this component is added to your scene, then any GmaeObjects that get spawned at runtime from CW code will
	///     be placed as children of this, rather than being placed in the scene root.
	/// </summary>
	[ExecuteInEditMode]
	[DefaultExecutionOrder(
		-100
	)]
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwRoot"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Root"
	)]
	public class CwRoot : MonoBehaviour
	{
		private readonly static List<CwRoot> instances = new();

		public static bool Exists => instances.Count > 0;

		public static Transform Root
		{
			get
			{
				if (instances.Count > 0) return instances[0].transform;

				return null;
			}
		}

		protected virtual void OnEnable()
		{
			if (instances.Count > 0)
				Debug.LogWarning(
					"Your scene already contains an instance of CwRoot!",
					instances[0]
				);

			instances.Add(
				this
			);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(
				this
			);
		}

		public static Transform GetRoot()
		{
			if (instances.Count == 0)
				new GameObject(
					"CwRoot"
				).AddComponent<CwRoot>();

			return instances[0].transform;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwRoot)
	)]
	public class CwRoot_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwRoot tgt;
			CwRoot[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Info(
				"If this component is added to your scene, then any GmaeObjects that get spawned at runtime from CW code will be placed as children of this, rather than being placed in the scene root."
			);
		}
	}

#endif
}