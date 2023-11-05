using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sources.Utils
{
	[RequireComponent(typeof(UIDocument))] public class VisualElementGetter : MonoBehaviour
	{
		private UIDocument _uiDocument;

		private UQueryBuilder<VisualElement> _queryBuilder;

		private void Awake() =>
			_uiDocument = GetComponent<UIDocument>();

		public T GetFirst<T>(string elementName) where T : VisualElement =>
			_uiDocument.rootVisualElement.Q<T>(elementName);

		public T GetFirstUIElementQuery<T>(string elementName) where T : VisualElement =>
			_uiDocument.rootVisualElement.Query<T>(elementName);

		public List<T> GetChildren<T>(string elementName) where T : VisualElement
		{
			VisualElement parentElement = _uiDocument.rootVisualElement.Q<VisualElement>(elementName);
			IEnumerable<VisualElement> children = parentElement.Children();

			return children
				.Select(child => child.Query<T>())
				.Select(dummy => (T)dummy)
				.ToList();
		}

		public List<T> GetAllByType<T>() where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			return _queryBuilder.OfType<T>().ToList();
		}

		public T GetFirstInChildren<T>(VisualElement parentElement, string elementName) where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			IEnumerable<VisualElement> children = parentElement.Children();

			return (
				from child
					in children
				where child.Q<VisualElement>(elementName) != null
				select child.Q<T>(elementName)).FirstOrDefault();
		}

		public List<T> GetAllElementsByName<T>(string elementName) where T : VisualElement
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			return _queryBuilder.OfType<T>().Where(elem => elem.name == elementName).ToList();
		}

		public List<VisualElement> GetAllElementsByName(string elementName)
		{
			_queryBuilder = new UQueryBuilder<VisualElement>(_uiDocument.rootVisualElement);
			return _queryBuilder.Where(elem => elem.name == elementName).ToList();
		}
	}
}