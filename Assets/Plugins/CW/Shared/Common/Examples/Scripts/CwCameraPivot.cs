﻿using System;
using Plugins.CW.Shared.Common.Extras.Scripts;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>
	///     This component allows you to rotate the current GameObject using local Euler rotations, allowing you to create
	///     a typical FPS camera system, or orbital camera system.
	/// </summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwCameraPivot"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Camera Pivot"
	)]
	public class CwCameraPivot : MonoBehaviour
	{
		[SerializeField] private bool listen = true;

		[SerializeField] private float damping = 10.0f;

		[SerializeField]
		private CwInputManager.Axis pitchControls = new(
			1,
			true,
			CwInputManager.AxisGesture.VerticalDrag,
			-0.1f,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			45.0f
		);

		[SerializeField]
		private CwInputManager.Axis yawControls = new(
			1,
			true,
			CwInputManager.AxisGesture.HorizontalDrag,
			0.1f,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			45.0f
		);

		[NonSerialized] private Vector3 remainingDelta;

		/// <summary>Is this component currently listening for inputs?</summary>
		public bool Listen
		{
			set => listen = value;
			get => listen;
		}

		/// <summary>How quickly the position transitions from the current to the target value (-1 = instant).</summary>
		public float Damping
		{
			set => damping = value;
			get => damping;
		}

		/// <summary>The keys/fingers required to pitch down/up.</summary>
		public CwInputManager.Axis PitchControls
		{
			set => pitchControls = value;
			get => pitchControls;
		}

		/// <summary>The keys/fingers required to yaw left/right.</summary>
		public CwInputManager.Axis YawControls
		{
			set => yawControls = value;
			get => yawControls;
		}

		protected virtual void Update()
		{
			if (listen) AddToDelta();

			DampenDelta();
		}

		protected virtual void OnEnable()
		{
			CwInputManager.EnsureThisComponentExists();
		}

		private void AddToDelta()
		{
			remainingDelta.x += pitchControls.GetValue(
				Time.deltaTime
			);
			remainingDelta.y += yawControls.GetValue(
				Time.deltaTime
			);
		}

		private void DampenDelta()
		{
			// Dampen remaining delta
			float factor = CwHelper.DampenFactor(
				damping,
				Time.deltaTime
			);
			Vector3 newDelta = Vector3.Lerp(
				remainingDelta,
				Vector3.zero,
				factor
			);

			// Rotate by difference
			Vector3 euler = transform.localEulerAngles;

			euler.x = -Mathf.DeltaAngle(
				euler.x,
				0.0f
			);

			euler += remainingDelta - newDelta;

			euler.x = Mathf.Clamp(
				euler.x,
				-89.0f,
				89.0f
			);

			transform.localEulerAngles = euler;

			// Update remaining
			remainingDelta = newDelta;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwCameraPivot)
	)]
	public class CwCameraPivot_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwCameraPivot tgt;
			CwCameraPivot[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"listen",
				"Is this component currently listening for inputs?"
			);
			Draw(
				"damping",
				"How quickly the rotation transitions from the current to the target value (-1 = instant)."
			);

			Separator();

			Draw(
				"pitchControls",
				"The keys/fingers required to pitch down/up."
			);
			Draw(
				"yawControls",
				"The keys/fingers required to yaw left/right."
			);
		}
	}

#endif
}