namespace ZigZag
{
	public class SpeedEffectFactory : IEffectFactory<SpeedEffect>
	{
		private readonly GamePlayService _gamePlay;

		private const int _baseEffectTime = 1;

		private const float _baseEffectSpeed = 1f;

		public SpeedEffectFactory(GamePlayService gamePlay)
		{
			_gamePlay = gamePlay;
		}

		public SpeedEffect Create()
		{
			return new SpeedEffect(
				_baseEffectTime,
				_baseEffectSpeed * _gamePlay.DifficultyLevel + 3f,
				_gamePlay
				);
		}
	}
}