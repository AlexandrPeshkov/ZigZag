namespace ZigZag
{
	/// <summary>
	/// Эффект бонусных очков
	/// </summary>
	public sealed class PointsEffect : IEffect
	{
		private readonly ScoreService _scoreService;

		public int Points { get; private set; }

		public PointsEffect(ScoreService scoreService, int points)
		{
			_scoreService = scoreService;
			Points = points;
		}

		public void Apply()
		{
			_scoreService.AddPoints(Points);
		}
	}
}