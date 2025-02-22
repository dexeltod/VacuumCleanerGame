﻿using System;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>This component turns the current UI element into a button that links to the specified action.</summary>
	[ExecuteInEditMode]
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwDemoButton"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Demo Button"
	)]
	public class CwDemoButton : MonoBehaviour, IPointerDownHandler
	{
		public enum LinkType
		{
			PreviousScene,
			NextScene,
			Publisher,
			URL,
			Isolate
		}

		public enum ToggleType
		{
			KeepSelected,
			ToggleSelection,
			SelectPrevious
		}

		[SerializeField] private LinkType link;

		[SerializeField] private string urlTarget;

		[SerializeField] private Transform isolateTarget;

		[SerializeField] private ToggleType isolateToggle;

		[NonSerialized] private CanvasGroup cachedCanvasGroup;

		[NonSerialized] private Transform previousChild;

		/// <summary>The action that will be performed when this UI element is clicked.</summary>
		public LinkType Link
		{
			set => link = value;
			get => link;
		}

		/// <summary>The URL that will be opened.</summary>
		public string UrlTarget
		{
			set => urlTarget = value;
			get => urlTarget;
		}

		/// <summary>If this GameObject is active, then the button will be faded in.</summary>
		public Transform IsolateTarget
		{
			set => isolateTarget = value;
			get => isolateTarget;
		}

		/// <summary>If this button is already selected and you click/tap it again, what should happen?</summary>
		public ToggleType IsolateToggle
		{
			set => isolateToggle = value;
			get => isolateToggle;
		}

		protected virtual void Update()
		{
			var group = GetComponent<CanvasGroup>();

			if (group != null)
			{
				var alpha = 1.0f;

				switch (link)
				{
					case LinkType.PreviousScene:
					case LinkType.NextScene:
					{
						alpha = GetCurrentLevel() >= 0 && GetLevelCount() > 1 ? 1.0f : 0.0f;
					}
						break;
					case LinkType.Isolate:
					{
						if (isolateTarget != null) alpha = isolateTarget.gameObject.activeInHierarchy ? 1.0f : 0.5f;
					}
						break;
				}

				group.alpha = alpha;
				group.blocksRaycasts = alpha > 0.0f;
				group.interactable = alpha > 0.0f;
			}
		}

		protected virtual void OnEnable()
		{
			cachedCanvasGroup = GetComponent<CanvasGroup>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			switch (link)
			{
				case LinkType.PreviousScene:
				{
					int index = GetCurrentLevel();

					if (index >= 0)
					{
						if (--index < 0) index = GetLevelCount() - 1;

						LoadLevel(
							index
						);
					}
				}
					break;

				case LinkType.NextScene:
				{
					int index = GetCurrentLevel();

					if (index >= 0)
					{
						if (++index >= GetLevelCount()) index = 0;

						LoadLevel(
							index
						);
					}
				}
					break;

				case LinkType.Publisher:
				{
					Application.OpenURL(
						"https://carloswilkes.com"
					);
				}
					break;

				case LinkType.URL:
				{
					if (string.IsNullOrEmpty(
						    urlTarget
					    ) ==
					    false)
						Application.OpenURL(
							urlTarget
						);
				}
					break;

				case LinkType.Isolate:
				{
					if (isolateTarget != null)
					{
						Transform parent = isolateTarget.transform.parent;
						bool active = isolateTarget.gameObject.activeSelf;

						foreach (Transform child in parent.transform)
							if (child.gameObject.activeSelf)
							{
								if (child != isolateTarget) previousChild = child;

								child.gameObject.SetActive(
									false
								);
							}

						switch (isolateToggle)
						{
							case ToggleType.KeepSelected:
							{
								isolateTarget.gameObject.SetActive(
									true
								);
							}
								break;

							case ToggleType.ToggleSelection:
							{
								isolateTarget.gameObject.SetActive(
									active == false
								);
							}
								break;

							case ToggleType.SelectPrevious:
							{
								if (active && previousChild != null)
									previousChild.gameObject.SetActive(
										true
									);
								else
									isolateTarget.gameObject.SetActive(
										true
									);
							}
								break;
						}
					}
				}
					break;
			}
		}

		private static int GetCurrentLevel()
		{
			Scene scene = SceneManager.GetActiveScene();
			int index = scene.buildIndex;

			if (index >= 0)
				if (SceneManager.GetSceneByBuildIndex(
					    index
				    ).handle !=
				    scene.handle)
					return -1;

			return index;
		}

		private static int GetLevelCount() =>
			SceneManager.sceneCountInBuildSettings;

		private static void LoadLevel(int index)
		{
			SceneManager.LoadScene(
				index
			);
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwDemoButton)
	)]
	public class CwDemoButton_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwDemoButton tgt;
			CwDemoButton[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"link",
				"The action that will be performed when this UI element is clicked."
			);

			BeginIndent();

			if (Any(
				    tgts,
				    t => t.Link == CwDemoButton.LinkType.URL
			    ))
				Draw(
					"urlTarget",
					"The URL that will be opened.",
					"Target"
				);

			if (Any(
				    tgts,
				    t => t.Link == CwDemoButton.LinkType.Isolate
			    ))
			{
				Draw(
					"isolateTarget",
					"If this GameObject is active, then the button will be faded in.",
					"Target"
				);
				Draw(
					"isolateToggle",
					"If this button is already selected and you click/tap it again, what should happen?",
					"Toggle"
				);
			}

			EndIndent();
		}
	}

#endif
}