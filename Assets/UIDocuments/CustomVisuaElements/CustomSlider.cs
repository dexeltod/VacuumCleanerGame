using UnityEngine;
using UnityEngine.UIElements;

public class CustomSlider : VisualElement
{
	public new class UxmlFactory : UxmlFactory<CustomSlider, UxmlTraits> { }

	public CustomSlider()
	{
		// Загрузка UXML
		var template = Resources.Load<VisualTreeAsset>("Slider");
		template.CloneTree(this);

		// Применение стилей из USS
		styleSheets.Add(Resources.Load<StyleSheet>("SliderStyle"));
	}
}