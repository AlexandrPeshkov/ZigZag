namespace ZigZag
{
	/// <summary>
	/// Эффект бонусных очков
	/// </summary>
	public sealed class PointsEffect : IEffect
	{
		private readonly ScoreService _scoreService;

		public int Points { get; private set; }

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Instantaneous;

		public PointsEffect(ScoreService scoreService)
		{
			_scoreService = scoreService;
		}

		public void Initialize(int points)
		{
			Points = points;
		}

		public void Apply()
		{
			_scoreService.AddPoints(Points);
		}

		public void Cancel()
		{
			//nothing
		}
	}
}