using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.PresentationInterfaces
{
	public interface IDissolveShaderView : IPresentableView<IDissolveShaderViewController>
	{
		void Construct(IDissolveShaderViewController controller);
		void SetDissolvingValue(float normalizedDissolvingValue);
	}
}