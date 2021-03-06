namespace ZigZag
{
	/// <summary>
	/// Дополнительная жизнь
	/// </summary>
	public class LifeEffect : IEffect
	{
		private readonly GamePlayService _gamePlayService;

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Instantaneous;

		public string Text => "+1";

		public LifeEffect(GamePlayService gamePlayService)
		{
			_gamePlayService = gamePlayService;
		}

		public void Apply()
		{
			_gamePlayService.AddLife();
		}

		public void Cancel()
		{
		}
	}
}