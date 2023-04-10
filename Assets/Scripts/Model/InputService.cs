using System;
using Model;

namespace Infrastructure.Services
{
	public class InputService : IInputService
	{
		public event Action<float> VerticalButtonUsed;
		public event Action VerticalButtonCanceled;
		public event Action AttackButtonUsed;
		public event Action InteractButtonUsed;
		public event Action JumpButtonUsed;
		public event Action JumpButtonCanceled;

		private readonly InputSystem _inputSystem;

		public InputService()
		{
			_inputSystem = new InputSystem();
			EnableInputs();
		}

		~InputService()
		{
			DisableInputs();
		}

		public void EnableInputs()
		{
			_inputSystem.Enable();
		}

		public void DisableInputs()
		{
			_inputSystem.Disable();
		}
	}
}