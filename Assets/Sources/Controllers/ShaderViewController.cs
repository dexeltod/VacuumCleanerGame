using System;
using System.Collections;
using Sources.Controllers.Common;
using UnityEngine;

namespace Sources.Presentation.Player
{
	public sealed class ShaderViewController : Presenter, IShaderViewController
	{
		private const float NormalizedDissolvingValue = 0;

		private readonly IShaderView _shaderView;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly WaitForSeconds _waitForSeconds;

		public ShaderViewController(IShaderView shaderView, ICoroutineRunnerProvider coroutineRunnerProvider)
		{
			_shaderView = shaderView ?? throw new ArgumentNullException(nameof(shaderView));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
		}

		public void StartDissolving() =>
			_coroutineRunnerProvider.Implementation.Run(DissolvingRoutine());

		private IEnumerator DissolvingRoutine()
		{
			float towards = 1f;

			while (towards > NormalizedDissolvingValue)
			{
				towards = Mathf.MoveTowards(towards, NormalizedDissolvingValue, 0.01f);

				Debug.Log($"Normalized: {towards}");

				_shaderView.SetDissolvingValue(towards);

				yield return null;
			}
		}
	}
}