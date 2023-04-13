using System;
using Model.Infrastructure.Services;

namespace Model.Infrastructure
{
	public interface IInputService : IService
	{
		event Action<float> VerticalButtonUsed;
		event Action VerticalButtonCanceled;
		event Action AttackButtonUsed;
		event Action InteractButtonUsed;
		event Action JumpButtonUsed;
		event Action JumpButtonCanceled;
	}
}