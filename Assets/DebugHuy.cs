using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugHuy : MonoBehaviour
{
	[SerializeField] private GraphicRaycaster _graphicRaycaster;
	private EventSystem _eventSystem;

	void Update()
	{
		_eventSystem = EventSystem.current;
		// Проверяем указатель мыши
		CheckUGUIElementUnderPointer();

		// Можно также сделать проверку каждый Update:
		// CheckUGUIElementUnderPointer();
	}

	private void CheckUGUIElementUnderPointer()
	{
		// Создаем PointerEventData с текущим положением мыши
		PointerEventData pointerEventData = new PointerEventData(_eventSystem)
		{
			position = Input.mousePosition
		};

		// Создаем список для сохранения результатов
		List<RaycastResult> results = new List<RaycastResult>();

		// Делаем raycast
		_graphicRaycaster.Raycast(pointerEventData, results);

		// Если есть результаты, выводим название первого элемента в лог
		foreach (RaycastResult result in results)
		{
			Debug.Log("UI Element: " + result.gameObject.name);
			// Можно добавить процедуру обработки этого элемента здесь
		}
	}
}