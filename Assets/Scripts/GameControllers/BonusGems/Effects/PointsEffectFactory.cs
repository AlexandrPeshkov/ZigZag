namespace ZigZag
{
	public class PointsEffectFactory : IEffectFactory<PointsEffect>
	{
		private const int _baseBonusPoints = 5;

		private readonly ScoreService _scoreService;

		private readonly GamePlayService _gamePlayService;

		public PointsEffectFactory(ScoreService scoreService, GamePlayService gamePlayService)
		{
			_scoreService = scoreService;
			_gamePlayService = gamePlayService;
		}

		public PointsEffect Create()
		{
			return new PointsEffect(_scoreService, _gamePlayService.DifficultyLevel * _baseBonusPoints);
		}
	}
}