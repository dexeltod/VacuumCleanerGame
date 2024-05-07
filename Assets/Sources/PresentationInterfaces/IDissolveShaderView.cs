using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Common;
using Sources.PresentationInterfaces.Common;

namespace Sources.PresentationInterfaces
{
	public interface IDissolveShaderView : IPresentableView<IDissolveShaderViewController>
	{
		void SetDissolvingValue(float normalizedDissolvingValue);
	}
}