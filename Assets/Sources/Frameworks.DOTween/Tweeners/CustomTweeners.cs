using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Sources.Frameworks.DOTween.Tweeners
{
	public static class CustomTweeners
	{
		public static TweenerCore<Vector3, Vector3, VectorOptions> StartPulseLocal(
			Transform objectTransform,
			float multiplier = 1.3f
		)
		{
			if (objectTransform == null)
				throw new ArgumentNullException(
					nameof(objectTransform)
				);

			return objectTransform
				.DOScale(
					objectTransform.localScale * multiplier,
					1f
				)
				.SetLoops(
					-1,
					LoopType.Yoyo
				);
		}
	}
}
