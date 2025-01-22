using System;
using Plugins.CW.Shared.Common.Extras.Scripts;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>This component allows you to freely move the current GameObject based on mouse/finger drags.</summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwCameraMove"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Camera Move"
	)]
	public class CwCameraMove : MonoBehaviour
	{
		[SerializeField] private bool listen = true;

		[SerializeField] private float damping = 10.0f;

		[SerializeField] private float sensitivity = 1.0f;

		[SerializeField]
		private CwInputManager.Axis horizontalControls = new(
			2,
			false,
			CwInputManager.AxisGesture.HorizontalDrag,
			1.0f,
			KeyCode.A,
			KeyCode.D,
			KeyCode.LeftArrow,
			KeyCode.RightArrow,
			100.0f
		);

		[SerializeField]
		private CwInputManager.Axis depthControls = new(
			2,
			false,
			CwInputManager.AxisGesture.HorizontalDrag,
			1.0f,
			KeyCode.S,
			KeyCode.W,
			KeyCode.DownArrow,
			KeyCode.UpArrow,
			100.0f
		);

		[SerializeField]
		private CwInputManager.Axis verticalControls = new(
			3,
			false,
			CwInputManager.AxisGesture.HorizontalDrag,
			1.0f,
			KeyCode.F,
			KeyCode.R,
			KeyCode.None,
			KeyCode.None,
			100.0f
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

		/// <summary>The movement speed will be multiplied by this.</summary>
		public float Sensitivity
		{
			set => sensitivity = value;
			get => sensitivity;
		}

		/// <summary>The keys/fingers required to move left/right.</summary>
		public CwInputManager.Axis HorizontalControls
		{
			set => horizontalControls = value;
			get => horizontalControls;
		}

		/// <summary>The keys/fingers required to move backward/forward.</summary>
		public CwInputManager.Axis DepthControls
		{
			set => depthControls = value;
			get => depthControls;
		}

		/// <summary>The keys/fingers required to move down/up.</summary>
		public CwInputManager.Axis VerticalControls
		{
			set => verticalControls = value;
			get => verticalControls;
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

		private void AddToDelta()
		{
			// Get delta from binds
			var delta = default(Vector3);

			delta.x = horizontalControls.GetValue(
				Time.deltaTime
			);
			delta.y = verticalControls.GetValue(
				Time.deltaTime
			);
			delta.z = depthControls.GetValue(
				Time.deltaTime
			);

			// Store old position
			Vector3 oldPosition = transform.position;

			// Translate
			transform.Translate(
				delta * sensitivity * Time.deltaTime,
				Space.Self
			);

			// Add to remaining
			Vector3 acceleration = transform.position - oldPosition;

			remainingDelta += acceleration;

			// Revert position
			transform.position = oldPosition;
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

			// Translate by difference
			transform.position += remainingDelta - newDelta;

			// Update remaining
			remainingDelta = newDelta;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwCameraMove)
	)]
	public class CwCameraMove_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwCameraMove tgt;
			CwCameraMove[] tgts;
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
			Draw(
				"sensitivity",
				"The movement speed will be multiplied by this."
			);

			Separator();

			Draw(
				"horizontalControls",
				"The keys/fingers required to move right/left."
			);
			Draw(
				"depthControls",
				"The keys/fingers required to move backward/forward."
			);
			Draw(
				"verticalControls",
				"The keys/fingers required to move down/up."
			);
		}
	}

#endif
}