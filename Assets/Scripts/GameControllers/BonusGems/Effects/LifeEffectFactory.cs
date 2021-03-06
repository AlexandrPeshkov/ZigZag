namespace ZigZag
{
	public class LifeEffectFactory : IEffectFactory<LifeEffect>
	{
		private GamePlayService _gamePlayService;

		public LifeEffectFactory(GamePlayService gamePlayService)
		{
			_gamePlayService = gamePlayService;
		}

		public LifeEffect Create()
		{
			return new LifeEffect(_gamePlayService);
		}
	}
}