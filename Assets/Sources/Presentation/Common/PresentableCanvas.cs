using Sources.ControllersInterfaces;
using Sources.Domain.Interfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.Common
{
	public abstract class PresentableCanvas<T> : PresentableView<T>, IPresentableCanvas<T> where T : class, IPresenter
	{
		public ITextPhrases TextPhrases { get; private set; }
		public RectTransform RectTransform { get; private set; }

		public void Construct(T presenter, RectTransform rectTransform = null, ITextPhrases textPhrases = null)
		{
			Presenter = presenter;

			if (rectTransform != null)
				RectTransform = rectTransform;

			if (TextPhrases != textPhrases)
				TextPhrases = textPhrases;
		}

		public void Construct(
			RectTransform rectTransform,
			T presenter = default,
			ITextPhrases textPhrases = null)
		{
			RectTransform = rectTransform;

			if (presenter != null)
				Presenter = presenter;

			if (TextPhrases != textPhrases)
				TextPhrases = textPhrases;
		}
	}
}