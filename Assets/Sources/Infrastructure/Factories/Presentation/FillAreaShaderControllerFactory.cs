using System;
using Sources.Controllers;
using Sources.Presentation;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Presentation
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