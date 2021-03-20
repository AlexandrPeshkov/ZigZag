using System.Threading;
using System.Threading.Tasks;

namespace ZigZag
{
	/// <summary>
	/// Эффект ускорения
	/// </summary>
	public sealed class SpeedEffect : IEffect
	{
		/// <summary>
		/// Бонус к скорости
		/// </summary>
		private readonly float _speed;

		/// <summary>
		/// Время действия
		/// </summary>
		private readonly int _seconds;

		private readonly GamePlayService _gamePlay;

		private readonly CancellationToken _cancellationToken;

		private readonly CancellationTokenSource _tokenSource;

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Temporary;

		public SpeedEffect(int seconds, float speed, GamePlayService gamePlay)
		{
			_gamePlay = gamePlay;
			_seconds = seconds;
			_speed = speed;

			_tokenSource = new CancellationTokenSource();
			_cancellationToken = _tokenSource.Token;
		}

		public void Apply()
		{
			Task.Run(ApplySpeed, _cancellationToken).ConfigureAwait(false).GetAwaiter();
		}

		public void Cancel()
		{
			_tokenSource.Cancel();
		}

		//TODO плавное замедление
		private async Task ApplySpeed()
		{
			_gamePlay.ChangeSpeedBonus(_speed);

			await Task.Delay(_seconds * 1000);

			if (_cancellationToken.IsCancellationRequested)
			{
				_cancellationToken.ThrowIfCancellationRequested();
			}
			_gamePlay.ChangeSpeedBonus(0);
		}
	}
}