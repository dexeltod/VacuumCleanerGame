using UnityEngine;

namespace SerializeInterfaces.Samples.Scripts
{
	public class InteractableComponent : MonoBehaviour, IInteractable
	{
		public string DebugText;

		public void Interact()
		{
			Debug.Log($"Interacted with component: {this.name}");
		}
	}
}
