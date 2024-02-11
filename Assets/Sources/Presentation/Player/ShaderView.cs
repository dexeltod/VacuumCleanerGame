using Sources.Presentation.Common;
using UnityEngine;

namespace Sources.Presentation.Player
{
	public class ShaderView : PresentableView<IShaderViewController>, IShaderView
	{
		[SerializeField] private Renderer[] _renderers;

		private const string DissolvingParameterName = "_Dissolve";

		private static readonly int Dissolve = Shader.PropertyToID(DissolvingParameterName);

		public void SetDissolvingValue(float normalizedDissolvingValue)
		{
			foreach (Renderer rendererComponent in _renderers)
				rendererComponent.material.SetFloat(Dissolve, normalizedDissolvingValue);
		}
	}
}