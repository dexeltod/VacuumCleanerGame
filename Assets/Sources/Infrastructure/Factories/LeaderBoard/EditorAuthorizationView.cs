using System;
using Sources.Controllers;
using Sources.Presentation;
using Sources.Presentation.Common;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class EditorAuthorizationView : PresentableView<IAuthorizationPresenter>, IAuthorizationView
	{
		public void Construct(AuthorizationPresenter presenter) =>
			throw new NotImplementedException();

		public override void Enable() =>
			throw new NotImplementedException();

		public override void Disable() =>
			throw new NotImplementedException();
	}
}