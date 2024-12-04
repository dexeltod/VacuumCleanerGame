using UnityEngine;

namespace Sources.Presentation
{
	public class ShaderEffectUpdater : MonoBehaviour
	{
		[SerializeField] private RenderTexture _renderTexture;
		[SerializeField] private Transform _target;

		private static readonly int GlobalEffectRT = Shader.PropertyToID("_GlobalEffectRT");
		private static readonly int OrthographicCamSize = Shader.PropertyToID("_OrthographicCamSize");
		private static readonly int Position = Shader.PropertyToID("_Position");

		// Start is called before the first frame update
		private void Awake()
		{
			Shader.SetGlobalTexture(GlobalEffectRT, _renderTexture);
			Shader.SetGlobalFloat(OrthographicCamSize, GetComponent<Camera>().orthographicSize);
		}

		private void Update()
		{
			transform.position = new Vector3(
				_target.transform.position.x,
				transform.position.y,
				_target.transform.position.z
			);
			Shader.SetGlobalVector(Position, transform.position);
		}
	}
}