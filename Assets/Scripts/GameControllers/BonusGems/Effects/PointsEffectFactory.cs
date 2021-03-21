using Zenject;

namespace ZigZag
{
	public class PointsEffectFactory : IEffectFactory<PointsEffect>
	{
		private const int _baseBonusPoints = 5;

		private readonly GamePlayService _gamePlayService;

		private readonly DiContainer _container;

		public PointsEffectFactory(GamePlayService gamePlayService, DiContainer container)
		{
			_gamePlayService = gamePlayService;
			_container = container;
		}

		public PointsEffect Create()
		{
			PointsEffect pointsEffect = _container.Resolve<PointsEffect>();

			pointsEffect.Initialize(points: _gamePlayService.DifficultyLevel * _baseBonusPoints);

			return pointsEffect;
		}
	}
}