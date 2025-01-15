using System;
using Sources.Controllers;
using Sources.Infrastructure.Common.Factory;
using UnityEngine;

namespace Sources.Presentation.Factories
{
	public class FillAreaShaderControllerFactory : Factory<FillMeshShaderController>

	{
		private readonly GameObject _player;

		public FillAreaShaderControllerFactory(GameObject player) =>
			_player = player ? player : throw new ArgumentNullException(nameof(player));

		public override FillMeshShaderController Create()
		{
			FillMeshShaderView meshShaderView = _player.GetComponent<FillMeshShaderView>();

			return new FillMeshShaderController(meshShaderView.MeshRenderer);
		}
	}
}