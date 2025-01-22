using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sources.Infrastructure.Services
{
	public class UIElementGetterFacade : MonoBehaviour
	{
		[SerializeField] private UIDocument _uiDocument;

		private UQueryBuilder<VisualElement> _queryBuilder;

		public T GetFirst<T>(string elementName) where T : VisualElement =>
			_uiDocument.rootVisualElement.Q<T>(elementName);

		public T GetFirstUIElementQuery<T>(string elementName) where T : VisualElement =>
			_uiDocument.rootVisualElement.Query<T>(elementName);

		public List<T> GetChildren<T>(string elementName) where T : VisualElement
		{
			if (string.IsNullOrWhiteSpace(elementName)) throw new ArgumentNullException(nameof(elementName));

			var parentElement = _uiDocument.rootVisualElement.Q<VisualElement>(elementName);
			IEnumerable<VisualElement> children = parentElement.Children();

			List<T> elements = new();

			foreach (VisualElement child in children)
				elements.Add(child.Query<T>());

			return elements;
		}

		public List<T> GetAllByType<T>() where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			List<T> elements = _queryBuilder.OfType<T>().ToList();
			return elements;
		}

		public T GetFirstInChildren<T>(VisualElement parentElement, string elementName) where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			IEnumerable<VisualElement> children = parentElement.Children();

			foreach (VisualElement child in children)
				if (child.Q<VisualElement>(elementName) != null)
					return child.Q<T>(elementName);

			return null;
		}

		public List<T> GetAllElementsByName<T>(string elementName) where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			List<T> elements = _queryBuilder.OfType<T>().Where(elem => elem.name == elementName).ToList();
			return elements;
		}

		public List<VisualElement> GetAllElementsByName(string elementName)
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			List<VisualElement> elements = _queryBuilder.Where(elem => elem.name == elementName).ToList();
			return elements;
		}
	}
}