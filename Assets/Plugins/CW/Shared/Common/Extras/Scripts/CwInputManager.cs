using System;
using System.Collections.Generic;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>This component combines finger and mouse and keyboard inputs into a single interface.</summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwInputManager"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Input Manager"
	)]
	public class CwInputManager : MonoBehaviour
	{
		public enum AxisGesture
		{
			HorizontalDrag,
			VerticalDrag,
			Twist,
			HorizontalPull,
			VerticalPull
		}

		public const int MOUSE_FINGER_INDEX = -1;

		public const int HOVER_FINGER_INDEX = -1337;

		private readonly static List<RaycastResult> tempRaycastResults = new(
			10
		);

		private static PointerEventData tempPointerEventData;

		private static EventSystem tempEventSystem;

		private readonly static List<Finger> filteredFingers = new();

		private readonly static Stack<Finger> pool = new();

		[SerializeField] private LayerMask guiLayers = 1 << 5;

		/// <summary>Fingers that began touching the screen on top of these UI layers will be ignored.</summary>
		public LayerMask GuiLayers
		{
			set => guiLayers = value;
			get => guiLayers;
		}

		public static List<Finger> Fingers { get; } = new();

		public static float ScaleFactor
		{
			get
			{
				float dpi = Screen.dpi;

				if (dpi <= 0) dpi = 200.0f;

				return 200.0f / dpi;
			}
		}

		protected virtual void Update()
		{
			// Remove previously up fingers, or mark them as up in case the up event isn't read correctly
			for (int i = Fingers.Count - 1; i >= 0; i--)
			{
				Finger finger = Fingers[i];

				if (finger.Up)
				{
					Fingers.RemoveAt(
						i
					);
					pool.Push(
						finger
					);
				}
				else
				{
					finger.Up = true;
				}
			}

			// Update real fingers
			if (CwInput.GetTouchCount() > 0)
			{
				for (var i = 0; i < CwInput.GetTouchCount(); i++)
				{
					int id;
					Vector2 position;
					float pressure;
					bool set;

					CwInput.GetTouch(
						i,
						out id,
						out position,
						out pressure,
						out set
					);

					AddFinger(
						id,
						position,
						pressure,
						set
					);
				}
			}
			// If there are no real touches, simulate some from the mouse?
			else if (CwInput.GetMouseExists())
			{
				var mouseSet = false;
				var mouseUp = false;

				for (var i = 0; i < 5; i++)
				{
					mouseSet |= CwInput.GetMouseIsHeld(
						i
					);
					mouseUp |= CwInput.GetMouseWentUp(
						i
					);
				}

				AddFinger(
					HOVER_FINGER_INDEX,
					CwInput.GetMousePosition(),
					0.0f,
					true
				);

				if (mouseSet || mouseUp)
					AddFinger(
						MOUSE_FINGER_INDEX,
						CwInput.GetMousePosition(),
						1.0f,
						mouseSet
					);
			}

			// Events
			foreach (Finger finger in Fingers)
			{
				if (finger.Down && OnFingerDown != null)
					OnFingerDown.Invoke(
						finger
					);
				if (OnFingerUpdate != null)
					OnFingerUpdate.Invoke(
						finger
					);
				if (finger.Up && OnFingerUp != null)
					OnFingerUp.Invoke(
						finger
					);
			}
		}

		/// <summary>This event will tell you when a finger begins touching the screen.</summary>
		public static event Action<Finger> OnFingerDown;

		/// <summary>This event will tell you when a finger has begun, is, or has just stopped touching the screen.</summary>
		public static event Action<Finger> OnFingerUpdate;

		/// <summary>This event will tell you when a finger stops touching the screen.</summary>
		public static event Action<Finger> OnFingerUp;

		public static List<Finger> GetFingers(bool ignoreStartedOverGui = false, bool ignoreHover = false)
		{
			filteredFingers.Clear();

			foreach (Finger finger in Fingers)
			{
				if (ignoreStartedOverGui && finger.StartedOverGui) continue;

				if (ignoreHover && finger.Index == HOVER_FINGER_INDEX) continue;

				filteredFingers.Add(
					finger
				);
			}

			return filteredFingers;
		}

		public static bool PointOverGui(Vector2 screenPosition, int guiLayers = 1 << 5) =>
			RaycastGui(
				screenPosition,
				guiLayers
			).Count >
			0;

		/// <summary>
		///     This method gives you all UI elements under the specified screen position, where element 0 is the first/top
		///     one.
		/// </summary>
		public static List<RaycastResult> RaycastGui(Vector2 screenPosition, int guiLayers = 1 << 5)
		{
			tempRaycastResults.Clear();

			EventSystem currentEventSystem = EventSystem.current;

			if (currentEventSystem != null)
			{
				// Create point event data for this event system?
				if (currentEventSystem != tempEventSystem)
				{
					tempEventSystem = currentEventSystem;

					if (tempPointerEventData == null)
						tempPointerEventData = new PointerEventData(
							tempEventSystem
						);
					else
						tempPointerEventData.Reset();
				}

				// Raycast event system at the specified point
				tempPointerEventData.position = screenPosition;

				currentEventSystem.RaycastAll(
					tempPointerEventData,
					tempRaycastResults
				);

				// Loop through all results and remove any that don't match the layer mask
				if (tempRaycastResults.Count > 0)
					for (int i = tempRaycastResults.Count - 1; i >= 0; i--)
					{
						RaycastResult raycastResult = tempRaycastResults[i];
						int raycastLayer = 1 << raycastResult.gameObject.layer;

						if ((raycastLayer & guiLayers) == 0)
							tempRaycastResults.RemoveAt(
								i
							);
					}
			}

			return tempRaycastResults;
		}

		public static Vector2 GetAveragePosition(List<Finger> fingers)
		{
			Vector2 total = Vector2.zero;

			foreach (Finger finger in fingers) total += finger.ScreenPosition;

			return fingers.Count == 0 ? total : total / fingers.Count;
		}

		public static Vector2 GetAverageOldPosition(List<Finger> fingers)
		{
			Vector2 total = Vector2.zero;

			foreach (Finger finger in fingers) total += finger.ScreenPositionOld;

			return fingers.Count == 0 ? total : total / fingers.Count;
		}

		public static Vector2 GetAveragePullScaled(List<Finger> fingers)
		{
			Vector2 total = Vector2.zero;

			foreach (Finger finger in fingers) total += finger.ScreenPosition - finger.StartScreenPosition;

			return fingers.Count == 0 ? total : total * ScaleFactor / fingers.Count;
		}

		public static Vector2 GetAverageDeltaScaled(List<Finger> fingers)
		{
			Vector2 total = Vector2.zero;

			foreach (Finger finger in fingers) total += finger.ScreenPosition - finger.ScreenPositionOld;

			return fingers.Count == 0 ? total : total * ScaleFactor / fingers.Count;
		}

		public static float GetAverageTwistRadians(List<Finger> fingers)
		{
			var total = 0.0f;
			Vector2 center = GetAveragePosition(
				fingers
			);
			Vector2 oldCenter = GetAverageOldPosition(
				fingers
			);

			foreach (Finger finger in fingers)
				total += GetDeltaRadians(
					finger,
					center,
					oldCenter
				);

			return fingers.Count == 0 ? total : total / fingers.Count;
		}

		/// <summary>
		///     If your component uses this component, then make sure you call this method at least once before you use it
		///     (e.g. from <b>Awake</b>).
		/// </summary>
		public static void EnsureThisComponentExists()
		{
			if (Application.isPlaying && CwHelper.FindAnyObjectByType<CwInputManager>() == null)
				new GameObject(
					typeof(CwInputManager).Name
				).AddComponent<CwInputManager>();
		}

		private Finger FindFinger(int index)
		{
			foreach (Finger finger in Fingers)
				if (finger.Index == index)
					return finger;

			return null;
		}

		private void AddFinger(int index, Vector2 screenPosition, float pressure, bool set)
		{
			Finger finger = FindFinger(
				index
			);

			if (finger == null)
			{
				finger = pool.Count > 0 ? pool.Pop() : new Finger();

				finger.Index = index;
				finger.Down = true;
				finger.Age = 0.0f;

				finger.StartedOverGui = PointOverGui(
					screenPosition,
					guiLayers
				);
				finger.StartScreenPosition = screenPosition;
				finger.ScreenPositionOld = screenPosition;
				finger.ScreenPositionOldOld = screenPosition;
				finger.ScreenPositionOldOldOld = screenPosition;

				Fingers.Add(
					finger
				);
			}
			else
			{
				finger.Down = false;
				finger.Age += Time.deltaTime;

				finger.ScreenPositionOldOldOld = finger.ScreenPositionOldOld;
				finger.ScreenPositionOldOld = finger.ScreenPositionOld;
				finger.ScreenPositionOld = finger.ScreenPosition;
			}

			finger.Pressure = pressure;
			finger.ScreenPosition = screenPosition;
			finger.Up = set == false;
		}

		private static Vector2 Hermite(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			float mu2 = t * t;
			float mu3 = mu2 * t;
			float x = HermiteInterpolate(
				a.x,
				b.x,
				c.x,
				d.x,
				t,
				mu2,
				mu3
			);
			float y = HermiteInterpolate(
				a.y,
				b.y,
				c.y,
				d.y,
				t,
				mu2,
				mu3
			);

			return new Vector2(
				x,
				y
			);
		}

		private static float HermiteInterpolate(float y0, float y1, float y2, float y3, float mu, float mu2, float mu3)
		{
			float m0 = (y1 - y0) * 0.5f + (y2 - y1) * 0.5f;
			float m1 = (y2 - y1) * 0.5f + (y3 - y2) * 0.5f;
			float a0 = 2.0f * mu3 - 3.0f * mu2 + 1.0f;
			float a1 = mu3 - 2.0f * mu2 + mu;
			float a2 = mu3 - mu2;
			float a3 = -2.0f * mu3 + 3.0f * mu2;

			return a0 * y1 + a1 * m0 + a2 * m1 + a3 * y2;
		}

		private static float GetRadians(Vector2 screenPosition, Vector2 referencePoint) =>
			Mathf.Atan2(
				screenPosition.x - referencePoint.x,
				screenPosition.y - referencePoint.y
			);

		private static float GetDeltaRadians(Finger finger, Vector2 referencePoint, Vector2 lastReferencePoint)
		{
			float a = GetRadians(
				finger.ScreenPositionOld,
				lastReferencePoint
			);
			float b = GetRadians(
				finger.ScreenPosition,
				referencePoint
			);
			float d = Mathf.Repeat(
				a - b,
				Mathf.PI * 2.0f
			);

			if (d > Mathf.PI) d -= Mathf.PI * 2.0f;

			return d;
		}

		[Serializable]
		public struct Axis
		{
			public int FingerCount;
			public bool FingerInvert;
			public AxisGesture FingerGesture;
			public float FingerSensitivity;

			public KeyCode KeyNegative;
			public KeyCode KeyPositive;
			public KeyCode KeyNegativeAlt;
			public KeyCode KeyPositiveAlt;
			public float KeySensitivity;

			public Axis(int fCount,
				bool fInvert,
				AxisGesture fGesture,
				float fSensitivty,
				KeyCode kNegative,
				KeyCode kPositive,
				KeyCode kNegativeAlt,
				KeyCode kPositiveAlt,
				float kSensitivity)
			{
				FingerCount = fCount;
				FingerInvert = fInvert;
				FingerGesture = fGesture;
				FingerSensitivity = fSensitivty;
				KeyNegative = kNegative;
				KeyPositive = kPositive;
				KeyNegativeAlt = kNegativeAlt;
				KeyPositiveAlt = kPositiveAlt;
				KeySensitivity = kSensitivity;
			}

			public float GetValue(float delta)
			{
				var value = 0.0f;
				List<Finger> fingers = GetFingers(
					true,
					true
				);
				var scale = 1.0f;

				value -= CwInput.GetKeyIsHeld(
					KeyNegative
				)
					? KeySensitivity * delta
					: 0.0f;
				value += CwInput.GetKeyIsHeld(
					KeyPositive
				)
					? KeySensitivity * delta
					: 0.0f;

				value -= CwInput.GetKeyIsHeld(
					KeyNegativeAlt
				)
					? KeySensitivity * delta
					: 0.0f;
				value += CwInput.GetKeyIsHeld(
					KeyPositiveAlt
				)
					? KeySensitivity * delta
					: 0.0f;

				if (FingerCount > 0 && fingers.Count == FingerCount)
				{
					if (FingerInvert && fingers[0].Index >= 0) scale = -1.0f;

					switch (FingerGesture)
					{
						case AxisGesture.HorizontalDrag:
							value += GetAverageDeltaScaled(
								         fingers
							         ).x *
							         FingerSensitivity *
							         scale;
							break;
						case AxisGesture.VerticalDrag:
							value += GetAverageDeltaScaled(
								         fingers
							         ).y *
							         FingerSensitivity *
							         scale;
							break;
						case AxisGesture.Twist:
							value += GetAverageTwistRadians(
								         fingers
							         ) *
							         FingerSensitivity;
							break;
						case AxisGesture.HorizontalPull:
							value += GetAveragePullScaled(
								         fingers
							         ).x *
							         FingerSensitivity *
							         delta *
							         scale;
							break;
						case AxisGesture.VerticalPull:
							value += GetAveragePullScaled(
								         fingers
							         ).y *
							         FingerSensitivity *
							         delta *
							         scale;
							break;
					}
				}

				return value;
			}
		}

		[Serializable]
		public struct Trigger
		{
			public bool UseFinger;
			public bool UseMouse;
			public KeyCode UseKey;

			public Trigger(bool uFinger, bool uMouse, KeyCode uKey)
			{
				UseFinger = uFinger;
				UseMouse = uMouse;
				UseKey = uKey;
			}

			public bool WentDown(Finger finger)
			{
				if (UseFinger && finger.Index >= 0 && finger.Down) return true;

				if (UseMouse && finger.Index == MOUSE_FINGER_INDEX && finger.Down) return true;

				if (UseKey != KeyCode.None &&
				    finger.Index == HOVER_FINGER_INDEX &&
				    CwInput.GetKeyWentDown(
					    UseKey
				    ))
					return true;

				return false;
			}

			public bool IsDown(Finger finger)
			{
				if (UseFinger && finger.Index >= 0 && finger.Up == false) return true;

				if (UseMouse && finger.Index == MOUSE_FINGER_INDEX && finger.Up == false) return true;

				if (UseKey != KeyCode.None &&
				    finger.Index == HOVER_FINGER_INDEX &&
				    CwInput.GetKeyIsHeld(
					    UseKey
				    ))
					return true;

				return false;
			}

			public bool WentUp(Finger finger, bool useAnyFinger = false)
			{
				if (useAnyFinger && finger.Up) return true;

				if (UseFinger && finger.Index >= 0 && finger.Up) return true;

				if (UseMouse && finger.Index == MOUSE_FINGER_INDEX && finger.Up) return true;

				if (UseKey != KeyCode.None &&
				    finger.Index == HOVER_FINGER_INDEX &&
				    CwInput.GetKeyWentUp(
					    UseKey
				    ))
					return true;

				return false;
			}
		}

		public abstract class Link
		{
			public Finger Finger;

			public virtual void Clear()
			{
			}

			public static void ClearAll<T>(List<T> links)
				where T : Link
			{
				if (links != null)
				{
					foreach (T link in links) link.Clear();

					links.Clear();
				}
			}

			public static void ClearAndRemove<T>(List<T> links, T link)
				where T : Link
			{
				if (link != null)
				{
					link.Clear();

					if (links != null)
						links.Remove(
							link
						);
				}
			}

			public static T Create<T>(ref List<T> links, Finger finger)
				where T : Link, new()
			{
				T link = Find(
					links,
					finger
				);

				if (link == null)
				{
					if (links == null) links = new List<T>();

					link = new T();

					link.Finger = finger;

					links.Add(
						link
					);
				}
				else
				{
					Debug.LogError(
						"Link already exists!"
					);
				}

				return link;
			}

			public static T Find<T>(List<T> links, Finger finger)
				where T : Link, new()
			{
				if (links != null)
					foreach (T link in links)
						if (link.Finger == finger)
							return link;

				return null;
			}
		}

		public class Finger
		{
			public float Age;
			public bool Down;
			public int Index;
			public float Pressure;
			public Vector2 ScreenPosition;
			public Vector2 ScreenPositionOld;
			public Vector2 ScreenPositionOldOld;
			public Vector2 ScreenPositionOldOldOld;
			public bool StartedOverGui;
			public Vector2 StartScreenPosition;
			public bool Up;

			public float SmoothScreenPositionDelta
			{
				get
				{
					if (Up == false)
						return Vector2.Distance(
							ScreenPositionOldOld,
							ScreenPositionOld
						);

					return Vector2.Distance(
						ScreenPositionOldOld,
						ScreenPosition
					);
				}
			}

			public Vector2 GetSmoothScreenPosition(float t)
			{
				if (Up == false)
					return Hermite(
						ScreenPositionOldOldOld,
						ScreenPositionOldOld,
						ScreenPositionOld,
						ScreenPosition,
						t
					);

				return Vector2.LerpUnclamped(
					ScreenPositionOldOld,
					ScreenPosition,
					t
				);
			}
		}
	}
}