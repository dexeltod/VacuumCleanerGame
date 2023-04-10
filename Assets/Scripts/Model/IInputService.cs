using System;

namespace Model
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