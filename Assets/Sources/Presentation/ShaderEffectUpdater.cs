using UnityEngine;

namespace Sources.Presentation
{
	public class ShaderEffectUpdater : MonoBehaviour
	{
		private readonly static int GlobalEffectRT = Shader.PropertyToID("_GlobalEffectRT");
		private readonly static int OrthographicCamSize = Shader.PropertyToID("_OrthographicCamSize");
		private readonly static int Position = Shader.PropertyToID("_Position");
		[SerializeField] private RenderTexture _renderTexture;
		[SerializeField] private Transform _target;

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