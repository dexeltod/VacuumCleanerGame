using Sources.PresentationInterfaces.Common;

namespace Sources.Presentation.Player
{
	public interface IShaderView : IPresentableView<IShaderViewController>
	{
		void Construct(IShaderViewController controller);
		void SetDissolvingValue(float normalizedDissolvingValue);
	}
}