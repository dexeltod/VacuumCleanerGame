using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.Player
{
	public class DissolveDissolveShaderView : PresentableView<IDissolveShaderViewController>, IDissolveShaderView
	{
		private const string DissolvingParameterName = "_Dissolve";

		private readonly static int Dissolve = Shader.PropertyToID(DissolvingParameterName);
		[SerializeField] private Renderer[] _renderers;

		public void SetDissolvingValue(float normalizedDissolvingValue)
		{
			foreach (Renderer rendererComponent in _renderers)
				rendererComponent.material.SetFloat(Dissolve, normalizedDissolvingValue);
		}
	}
}