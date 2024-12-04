using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IFillMeshShaderView
	{
		float MaxFillArea { get; }
		float MinFillArea { get; }
		MeshRenderer MeshRenderer { get; }
	}
}