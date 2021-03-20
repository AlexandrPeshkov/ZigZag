﻿namespace ZigZag
{
	/// <summary>
	/// Дополнительная жизнь
	/// </summary>
	public class LifeEffect : IEffect
	{
		private readonly GamePlayService _gamePlayService;

		public EffectLifecycle EffectLifecycle => EffectLifecycle.Instantaneous;

		public int LifesCount { get; private set; }

		public LifeEffect(GamePlayService gamePlayService, int lifes)
		{
			_gamePlayService = gamePlayService;
			LifesCount = lifes;
		}

		public void Apply()
		{
			_gamePlayService.AddLife();
		}

		public void Cancel()
		{
			//nothing
		}
	}
}