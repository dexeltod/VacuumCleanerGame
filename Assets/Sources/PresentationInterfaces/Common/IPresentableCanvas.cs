using Sources.ControllersInterfaces;
using Sources.Domain.Interfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableCanvas<in T> : IPresentableView<T> where T : class, IPresenter
	{
		RectTransform RectTransform { get; }
		void Construct(T presenter, RectTransform rectTransform = null, ITextPhrases textPhrases = null);
		void Construct(RectTransform rectTransform, T presenter = null, ITextPhrases textPhrases = null);
	}
}
