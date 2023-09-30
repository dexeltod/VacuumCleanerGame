using System;

namespace Sources.Services
{
	public interface IMeshDeformationPresenter
	{
		event Action<int> MeshDeformed;
	}
}