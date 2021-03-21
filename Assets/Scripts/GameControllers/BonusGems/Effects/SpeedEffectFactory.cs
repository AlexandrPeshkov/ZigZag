using Zenject;

namespace ZigZag
{
	public class SpeedEffectFactory : IEffectFactory<SpeedEffect>
	{
		private const int _baseEffectTime = 5;

		private const float _baseEffectSpeed = 1f;

		private readonly GamePlayService _gamePlay;

		private readonly DiContainer _container;

		public SpeedEffectFactory(GamePlayService gamePlay, DiContainer container)
		{
			_gamePlay = gamePlay;
			_container = container;
		}

		public SpeedEffect Create()
		{
			SpeedEffect speedEffect = _container.Resolve<SpeedEffect>();

			speedEffect.Initialize(
				seconds: _baseEffectTime,
				speed: _baseEffectSpeed * _gamePlay.DifficultyLevel + 3f);

			return speedEffect;
		}
	}
}