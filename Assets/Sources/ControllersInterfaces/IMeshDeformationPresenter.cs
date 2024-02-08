using System;

namespace Sources.ControllersInterfaces
{
	public interface IMeshDeformationPresenter
	{
		event Action<int> MeshDeformed;
	}
}