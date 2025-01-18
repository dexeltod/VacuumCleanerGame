using System;
using Sources.Controllers;
using UnityEngine;

namespace Sources.Presentation.Factories
{
	public class FillAreaShaderControllerFactory

	{
		private readonly GameObject _player;

		public FillAreaShaderControllerFactory(GameObject player) =>
			_player = player ? player : throw new ArgumentNullException(nameof(player));

		public FillMeshShaderController Create()
		{
			FillMeshShaderView meshShaderView = _player.GetComponent<FillMeshShaderView>();

			return new FillMeshShaderController(meshShaderView.MeshRenderer);
		}
	}
}
