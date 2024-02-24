using System;
using System.Collections;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public sealed class DissolveShaderViewController : Presenter, IDissolveShaderViewController
	{
		private const float NormalizedDissolvingValue = 0;
		private const float Delta = 0.01f;

		private readonly IDissolveShaderView _dissolveShaderView;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly WaitForSeconds _waitForSeconds;

		public DissolveShaderViewController(
			IDissolveShaderView dissolveShaderView,
			ICoroutineRunnerProvider coroutineRunnerProvider
		)
		{
			_dissolveShaderView = dissolveShaderView ?? throw new ArgumentNullException(nameof(dissolveShaderView));
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
				towards = Mathf.MoveTowards(towards, NormalizedDissolvingValue, Delta);

				_dissolveShaderView.SetDissolvingValue(towards);

				yield return null;
			}
		}
	}
}