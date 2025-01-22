using System;
using Sources.Controllers;
using Sources.Presentation;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class FillAreaShaderControllerFactory

	{
		private readonly GameObject _player;

		public FillAreaShaderControllerFactory(GameObject player) =>
			_player = player ? player : throw new ArgumentNullException(nameof(player));

		public FillMeshShader Create()
		{
			var meshShaderView = _player.GetComponent<FillMeshShaderView>();

			return new FillMeshShader(meshShaderView.MeshRenderer);
		}
	}
}