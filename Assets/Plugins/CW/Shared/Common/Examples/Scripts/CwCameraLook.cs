﻿using System;
using Plugins.CW.Shared.Common.Extras.Scripts;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>This component allows you to freely rotate the current GameObject using local rotations.</summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwCameraLook"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Camera Look"
	)]
	public class CwCameraLook : MonoBehaviour
	{
		[SerializeField] private bool listen = true;

		[SerializeField] private float damping = 10.0f;

		[SerializeField] private float sensitivity = 1.0f;

		[SerializeField]
		private CwInputManager.Axis pitchControls = new(
			1,
			true,
			CwInputManager.AxisGesture.VerticalDrag,
			-10.0f,
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
			10.0f,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			KeyCode.None,
			45.0f
		);

		[SerializeField]
		private CwInputManager.Axis rollControls = new(
			2,
			true,
			CwInputManager.AxisGesture.Twist,
			1.0f,
			KeyCode.E,
			KeyCode.Q,
			KeyCode.None,
			KeyCode.None,
			45.0f
		);

		[NonSerialized] private Quaternion remainingDelta = Quaternion.identity;

		/// <summary>Is this component currently listening for inputs?</summary>
		public bool Listen
		{
			set => listen = value;
			get => listen;
		}

		/// <summary>How quickly the rotation transitions from the current to the target value (-1 = instant).</summary>
		public float Damping
		{
			set => damping = value;
			get => damping;
		}

		/// <summary>How quickly the mouse/finger movements rotate the camera.</summary>
		public float Sensitivity
		{
			set => sensitivity = value;
			get => sensitivity;
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

		/// <summary>The keys/fingers required to roll left/right.</summary>
		public CwInputManager.Axis RollControls
		{
			set => rollControls = value;
			get => rollControls;
		}

		protected virtual void Start()
		{
			CwInputManager.EnsureThisComponentExists();
		}

		protected virtual void Update()
		{
			if (listen) AddToDelta();

			DampenDelta();
		}

		protected virtual void OnDisable()
		{
			//oldMousePositionSet = false;
		}

		protected virtual void OnApplicationFocus(bool focus)
		{
			//oldMousePositionSet = false;
		}

		private void AddToDelta()
		{
			// Get delta from binds
			var delta = default(Vector3);

			delta.x = pitchControls.GetValue(
				Time.deltaTime
			);
			delta.y = yawControls.GetValue(
				Time.deltaTime
			);
			delta.z = rollControls.GetValue(
				Time.deltaTime
			);

			delta *= sensitivity;

			// Store old rotation
			Quaternion oldRotation = transform.localRotation;

			// Rotate
			transform.Rotate(
				delta.x,
				delta.y,
				0.0f,
				Space.Self
			);

			transform.Rotate(
				0.0f,
				0.0f,
				delta.z,
				Space.Self
			);

			// Add to remaining
			remainingDelta *= Quaternion.Inverse(
				                  oldRotation
			                  ) *
			                  transform.localRotation;

			// Revert rotation
			transform.localRotation = oldRotation;
		}

		private void DampenDelta()
		{
			// Dampen remaining delta
			float factor = CwHelper.DampenFactor(
				damping,
				Time.deltaTime
			);
			Quaternion newDelta = Quaternion.Slerp(
				remainingDelta,
				Quaternion.identity,
				factor
			);

			// Rotate by difference
			transform.localRotation = transform.localRotation *
			                          Quaternion.Inverse(
				                          newDelta
			                          ) *
			                          remainingDelta;

			// Update remaining
			remainingDelta = newDelta;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwCameraLook)
	)]
	public class CwCameraLook_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwCameraLook tgt;
			CwCameraLook[] tgts;
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
			Draw(
				"rollControls",
				"The keys/fingers required to roll left/right."
			);
		}
	}

#endif
}