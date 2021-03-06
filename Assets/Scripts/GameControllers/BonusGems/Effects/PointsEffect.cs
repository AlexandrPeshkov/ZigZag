namespace ZigZag
{
	/// <summary>
	/// Эффект бонусных очков
	/// </summary>
	public sealed class PointsEffect : IEffect
	{
		private readonly ScoreService _scoreService;

		private readonly int _points;

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Instantaneous;

		public string Text => $"+{_points}";

		public PointsEffect(ScoreService scoreService, int points)
		{
			_scoreService = scoreService;
			_points = points;
		}

		public void Apply()
		{
			_scoreService.AddPoints(_points);
		}

		public void Cancel()
		{
			//nothing
		}
	}
}