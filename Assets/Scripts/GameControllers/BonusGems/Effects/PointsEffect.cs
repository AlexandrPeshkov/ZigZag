namespace ZigZag
{
	public class PointsEffect : IEffect
	{
		private readonly ScoreService _scoreService;

		private readonly int _points;

		public PointsEffect(ScoreService scoreService, int points)
		{
			_scoreService = scoreService;
			_points = points;
		}

		public void Apply()
		{
			_scoreService.AddPoints(_points);
		}
	}
}