using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.PresentationInterfaces
{
	public interface IDissolveShaderView : IPresentableView<IDissolveShaderViewController>
	{
		new void Construct(IDissolveShaderViewController controller);
		void SetDissolvingValue(float normalizedDissolvingValue);
	}
}