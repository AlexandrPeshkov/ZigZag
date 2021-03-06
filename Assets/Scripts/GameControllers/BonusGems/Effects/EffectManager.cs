using System;
using System.Collections.Generic;
using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Менеджер эффектов
	/// </summary>
	public class EffectManager
	{
		/// <summary>
		/// Действующие временные эффекты
		/// </summary>
		private Dictionary<Type, IEffect> _activeTemporaryEffects;

		private DiContainer _container;

		public EffectManager(DiContainer container)
		{
			_container = container;
			_activeTemporaryEffects = new Dictionary<Type, IEffect>();
		}

		public TEffect ApplyEffect<TEffect>() where TEffect : IEffect
		{
			IEffectFactory<TEffect> effectFactory = _container.Resolve<IEffectFactory<TEffect>>();
			TEffect effect = effectFactory.Create();

			switch (effect.EffectLifecycle)
			{
				case EffectLifecycle.Instantaneous:
					{
						effect.Apply();
						break;
					}
				case EffectLifecycle.Temporary:
					{
						var effectType = typeof(TEffect);
						if (_activeTemporaryEffects.TryGetValue(effectType, out var activeEffect))
						{
							activeEffect.Cancel();
							_activeTemporaryEffects.Remove(effectType);
						}

						_activeTemporaryEffects.Add(effectType, effect);
						effect.Apply();
						break;
					}
			}

			return effect;
		}
	}
}