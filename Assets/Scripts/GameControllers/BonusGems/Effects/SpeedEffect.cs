using System.Threading.Tasks;

namespace ZigZag
{
	public class SpeedEffect : IEffect
	{
		/// <summary>
		/// Бонус к скорости
		/// </summary>
		private float _speed;

		/// <summary>
		/// Время действия
		/// </summary>
		private int _seconds;

		private GamePlayService _gamePlay;

		public SpeedEffect(int seconds, float speed, GamePlayService gamePlay)
		{
			_gamePlay = gamePlay;
			_seconds = seconds;
			_speed = speed;
		}

		//TODO прошлый эффект еще не отработал и отменяет замедление подобранного
		//Сделать коротиной или задачей с отменой
		public void Apply()
		{
			ApplySpeed().ConfigureAwait(false).GetAwaiter();
		}

		//TODO плавное замедление
		private async Task ApplySpeed()
		{
			_gamePlay.ChangeSpeedBonus(_speed);

			await Task.Delay(_seconds * 1000);

			_gamePlay.ChangeSpeedBonus(0);
		}
	}
}