using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>
	///     This component is used by all the demo scenes to perform common tasks. Including modifying the current scene
	///     to make it look consistent between different rendering pipelines.
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu(
		""
	)]
	public class CwDemo : MonoBehaviour
	{
		[SerializeField] private bool upgradeInputModule = true;

		[SerializeField] private bool changeExposureInHDRP = true;

		[SerializeField] private bool changeVisualEnvironmentInHDRP = true;

		[SerializeField] private bool changeFogInHDRP = true;

		[SerializeField] private bool changeCloudsInHDRP = true;

		[SerializeField] private bool changeMotionBlurInHDRP = true;

		[SerializeField] private bool upgradeLightsInHDRP = true;

		[SerializeField] private bool upgradeCamerasInHDRP = true;

		/// <summary>
		///     If you enable this setting and your project is running with the new InputSystem then the
		///     <b>EventSystem's InputModule</b> component will be upgraded.
		/// </summary>
		public bool UpgradeInputModule
		{
			set => upgradeInputModule = value;
			get => upgradeInputModule;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added
		///     to the scene that adjusts the camera exposure to match the other pipelines.
		/// </summary>
		public bool ChangeExposureInHDRP
		{
			set => changeExposureInHDRP = value;
			get => changeExposureInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added
		///     to the scene that adjusts the background to match the other pipelines.
		/// </summary>
		public bool ChangeVisualEnvironmentInHDRP
		{
			set => changeVisualEnvironmentInHDRP = value;
			get => changeVisualEnvironmentInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added
		///     to the scene that adjusts the fog to match the other pipelines.
		/// </summary>
		public bool ChangeFogInHDRP
		{
			set => changeFogInHDRP = value;
			get => changeFogInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added
		///     to the scene that adjusts the clouds to match the other pipelines.
		/// </summary>
		public bool ChangeCloudsInHDRP
		{
			set => changeCloudsInHDRP = value;
			get => changeCloudsInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added
		///     to the scene that adjusts the motion blur to match the other pipelines.
		/// </summary>
		public bool ChangeMotionBlurInHDRP
		{
			set => changeMotionBlurInHDRP = value;
			get => changeMotionBlurInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then any lights missing the
		///     <b>HDAdditionalLightData</b> component will have it added.
		/// </summary>
		public bool UpgradeLightsInHDRP
		{
			set => upgradeLightsInHDRP = value;
			get => upgradeLightsInHDRP;
		}

		/// <summary>
		///     If you enable this setting and your project is running with HDRP then any cameras missing the
		///     <b>HDAdditionalCameraData</b> component will have it added.
		/// </summary>
		public bool UpgradeCamerasInHDRP
		{
			set => upgradeCamerasInHDRP = value;
			get => upgradeCamerasInHDRP;
		}

		protected virtual void OnEnable()
		{
			if (upgradeInputModule) TryUpgradeEventSystem();

			if (CwHelper.IsURP) TryApplyURP();

			if (CwHelper.IsHDRP) TryApplyHDRP();
		}

		protected virtual void TryApplyURP()
		{
		}

		protected virtual void TryApplyHDRP()
		{
			if (changeExposureInHDRP || changeVisualEnvironmentInHDRP || changeFogInHDRP) TryCreateVolume();

			if (upgradeLightsInHDRP) TryUpgradeLights();

			if (upgradeCamerasInHDRP) TryUpgradeCameras();
		}

		private void TryCreateVolume()
		{
#if __HDRP__
			var volume = GetComponent<Volume>();

			if (volume == null)
			{
				volume = gameObject.AddComponent<Volume>();
			}

			var profile = volume.profile;

			if (profile == null)
			{
				profile = ScriptableObject.CreateInstance<VolumeProfile>();

				profile.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
			}

			if (profile.components.Count == 0)
			{
				name = "Demo (Volume Added)";

				if (changeExposureInHDRP == true)
				{
					var exposure = profile.Add<UnityEngine.Rendering.HighDefinition.Exposure>(true);

					exposure.fixedExposure.value = 14.0f;
				}

				if (changeVisualEnvironmentInHDRP == true)
				{
					var visualEnvironment = profile.Add<UnityEngine.Rendering.HighDefinition.VisualEnvironment>(true);

					visualEnvironment.skyType.value = 0;
				}

				if (changeFogInHDRP == true)
				{
					var fog = profile.Add<UnityEngine.Rendering.HighDefinition.Fog>(true);

					fog.enabled.value = false;
				}

	#if UNITY_2021_2_OR_NEWER
					if (changeCloudsInHDRP == true)
					{
						var clouds = profile.Add<UnityEngine.Rendering.HighDefinition.VolumetricClouds>(true);

						clouds.enable.value = false;
					}
	#endif

				if (changeMotionBlurInHDRP == true)
				{
					var motionBlur = profile.Add<UnityEngine.Rendering.HighDefinition.MotionBlur>(true);

					motionBlur.intensity.value = 0.0f;
				}
			}

			volume.profile = profile;
#endif
		}

		private void TryUpgradeLights()
		{
#if __HDRP__
			foreach (var light in CwHelper.FindObjectsByType<Light>())
			{
				if (light.GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalLightData>() == null)
				{
					light.gameObject.AddComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalLightData>();
				}
			}
#endif
		}

		private void TryUpgradeCameras()
		{
#if __HDRP__
			foreach (var camera in CwHelper.FindObjectsByType<Camera>())
			{
				if (camera.GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>() == null)
				{
					var hdCamera =
 camera.gameObject.AddComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>();

					hdCamera.backgroundColorHDR = Color.black;
				}
			}
#endif
		}

		private void TryUpgradeEventSystem()
		{
#if UNITY_EDITOR && ENABLE_INPUT_SYSTEM && __INPUTSYSTEM__
			var module = CwHelper.FindAnyObjectByType<StandaloneInputModule>();

			if (module != null)
			{
				module.gameObject.AddComponent<InputSystemUIInputModule>();

				DestroyImmediate(
					module
				);
			}
#endif
		}
	}

#if UNITY_EDITOR
	[CustomEditor(
		typeof(CwDemo)
	)]
	public class CwDemo_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwDemo tgt;
			CwDemo[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"upgradeInputModule",
				"If you enable this setting and your project is running with the new InputSystem then the EventSystem's InputModule component will be upgraded."
			);

			Separator();

			Draw(
				"changeExposureInHDRP",
				"If you enable this setting and your project is running with HDRP then a Volume component will be added to this GameObject that adjusts the camera exposure to match the other pipelines."
			);
			Draw(
				"changeVisualEnvironmentInHDRP",
				"If you enable this setting and your project is running with HDRP then a Volume component will be added to this GameObject that adjusts the background to match the other pipelines."
			);
			Draw(
				"changeFogInHDRP",
				"If you enable this setting and your project is running with HDRP then a Volume component will be added to the scene that adjusts the fog to match the other pipelines."
			);
			Draw(
				"changeCloudsInHDRP",
				"If you enable this setting and your project is running with HDRP then a Volume component will be added to the scene that adjusts the clouds to match the other pipelines."
			);
			Draw(
				"changeMotionBlurInHDRP",
				"If you enable this setting and your project is running with HDRP then a <b>Volume</b> component will be added to the scene that adjusts the motion blur to match the other pipelines."
			);
			Draw(
				"upgradeLightsInHDRP",
				"If you enable this setting and your project is running with HDRP then any lights missing the HDAdditionalLightData component will have it added."
			);
			Draw(
				"upgradeCamerasInHDRP",
				"If you enable this setting and your project is running with HDRP then any cameras missing the HDAdditionalCameraData component will have it added."
			);
		}
	}

#endif
}