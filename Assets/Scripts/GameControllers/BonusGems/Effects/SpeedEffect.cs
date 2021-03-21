using System.Collections;
using UnityEngine;
using ZigZag.Services;

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
		private float Speed { get; set; }

		/// <summary>
		/// Время действия
		/// </summary>
		private int Seconds { get; set; }

		private readonly GamePlayService _gamePlay;

		private readonly CoroutineService _coroutineService;

		private bool _cancelation;

		private Coroutine _coroutine;

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Temporary;

		public SpeedEffect(GamePlayService gamePlay, CoroutineService coroutineService)
		{
			_coroutineService = coroutineService;
			_gamePlay = gamePlay;
		}

		public void Initialize(int seconds, float speed)
		{
			Seconds = seconds;
			Speed = speed;
		}

		public void Apply()
		{
			_coroutine = _coroutineService.StartCoroutine(ApplySpeed());
		}

		public void Cancel()
		{
			_cancelation = true;
			_coroutineService.StopCoroutine(_coroutine);
		}

		private IEnumerator ApplySpeed()
		{
			_gamePlay.ChangeSpeedBonus(Speed);

			yield return new WaitForSeconds(Seconds);

			//if (_cancelation)
			//{
			//	yield break;
			//}

			_gamePlay.ChangeSpeedBonus(0);
		}
	}
}