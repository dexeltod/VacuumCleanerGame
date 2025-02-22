﻿using System;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>This component allows you to control a Camera component's depthTextureMode setting.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(
		typeof(Camera)
	)]
	[AddComponentMenu(
		"CW/Common/CW Depth Texture Mode"
	)]
	public class CwDepthTextureMode : MonoBehaviour
	{
		[SerializeField] private DepthTextureMode depthMode = DepthTextureMode.None;

		[NonSerialized] private Camera cachedCamera;

		/// <summary>The depth mode that will be applied to the camera.</summary>
		public DepthTextureMode DepthMode
		{
			set
			{
				depthMode = value;
				UpdateDepthMode();
			}
			get => depthMode;
		}

		protected virtual void Update()
		{
			UpdateDepthMode();
		}

		public void UpdateDepthMode()
		{
			if (cachedCamera == null) cachedCamera = GetComponent<Camera>();

			cachedCamera.depthTextureMode = depthMode;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwDepthTextureMode)
	)]
	public class CwDepthTextureMode_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwDepthTextureMode tgt;
			CwDepthTextureMode[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"depthMode",
				"The depth mode that will be applied to the camera."
			);
		}

		public static void RequireDepth()
		{
			var found = false;

			foreach (Camera camera in Camera.allCameras)
			{
				DepthTextureMode mask = camera.depthTextureMode;

				if (mask == DepthTextureMode.DepthNormals || ((int)mask & 1) != 0)
				{
					found = true;
					break;
				}
			}

			if (found == false)
			{
				Separator();

				if (Camera.main != null)
				{
					if (WritesDepth(
						    Camera.main
					    ) ==
					    false)
						if (HelpButton(
							    "This component requires your camera to render a Depth Texture, but it doesn't.",
							    MessageType.Error,
							    "Fix",
							    50.0f
						    ))
						{
							CwHelper.GetOrAddComponent<CwDepthTextureMode>(
									Camera.main.gameObject
								).DepthMode =
								DepthTextureMode.Depth;

							CwHelper.SelectAndPing(
								Camera.main
							);
						}
				}
				else
				{
					Error(
						"This component requires your camera to render a Depth Texture, but none of the cameras in your scene do. This can be fixed with the SgtDepthTextureMode component."
					);

					foreach (Camera camera in Camera.allCameras)
						if (CwHelper.Enabled(
							    camera
						    ))
						{
							CwHelper.GetOrAddComponent<CwDepthTextureMode>(
									camera.gameObject
								).DepthMode =
								DepthTextureMode.Depth;

							CwHelper.SelectAndPing(
								camera
							);
						}
				}
			}
		}

		private static bool WritesDepth(Camera camera) =>
			camera != null && camera.depthTextureMode == DepthTextureMode.DepthNormals ||
			((int)camera.depthTextureMode & 1) != 0;
	}

#endif
}