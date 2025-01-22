using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Plugins.SerializeInterfaces.Editor.UIElements
{
	internal class ToggleGroup
	{
		private readonly List<Toggle> _toggles = new();

		public Toggle GetFirstActiveToggle()
		{
			return _toggles.Find(
				x => x.value
			);
		}

		public bool IsAnyOn() =>
			GetFirstActiveToggle() != null;

		public void RegisterToggle(Toggle toggle)
		{
			if (toggle == null ||
			    _toggles.Contains(
				    toggle
			    )) return;

			_toggles.Add(
				toggle
			);
			toggle.RegisterValueChangedCallback(
				ToggleValueChanged
			);
		}

		public void UnregisterToggle(Toggle toggle)
		{
			if (!_toggles.Remove(
				    toggle
			    )) return;

			toggle.UnregisterValueChangedCallback(
				ToggleValueChanged
			);
		}

		public void Validate()
		{
			if (_toggles.Count == 0) return;

			Toggle activeToggle = GetFirstActiveToggle();

			if (activeToggle == null)
			{
				activeToggle = _toggles[0];
				activeToggle.value = true;
			}

			foreach (Toggle toggle in _toggles)
				if (toggle.value)
					toggle.SetValueWithoutNotify(
						false
					);
		}

		private void HandleToggleChanged(Toggle targetToggle)
		{
			ValidateToggleIsInGroup(
				targetToggle
			);

			foreach (Toggle toggle in _toggles)
			{
				if (toggle == targetToggle)
					continue;

				toggle.SetValueWithoutNotify(
					false
				);
			}

			if (targetToggle.value)
				OnToggleChanged?.Invoke(
					this,
					targetToggle
				);
			else
				targetToggle.value = true;
		}

		private void ToggleValueChanged(ChangeEvent<bool> evt)
		{
			HandleToggleChanged(
				evt.target as Toggle
			);
		}

		private void ValidateToggleIsInGroup(Toggle toggle)
		{
			if (toggle == null ||
			    !_toggles.Contains(
				    toggle
			    ))
				throw new ArgumentException(
					string.Format(
						"Toggle {0} is not part of ToggleGroup {1}",
						new object[]
						{
							toggle,
							this
						}
					)
				);
		}

		public event EventHandler<Toggle> OnToggleChanged;
	}
}