namespace Model
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}