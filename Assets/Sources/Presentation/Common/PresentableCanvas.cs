using System;
using Sources.ControllersInterfaces.Common;
using Sources.PresentationInterfaces.Common;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Presentation.Common
{
	public abstract class PresentableCanvas<T> : PresentableView<T>, IPresentableCanvas<T> where T : class, IPresenter
	{
		public RectTransform RectTransform { get; private set; }
		public ITextPhrases TextPhrases { get; private set; }

		public void Construct(T presenter, RectTransform rectTransform = null, ITextPhrases textPhrases = null)
		{
			Presenter = presenter;

			if (rectTransform != null)
				RectTransform = rectTransform;

			if (TextPhrases != textPhrases)
				TextPhrases = textPhrases;
		}

		public void Construct(RectTransform rectTransform, T presenter = default(T), ITextPhrases textPhrases = null)
		{
			RectTransform = rectTransform;

			if (presenter != default(T))
				Presenter = presenter;

			if (TextPhrases != textPhrases)
				TextPhrases = textPhrases;
		}
	}
}