using System;
using UnityEngine;
using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Базовый контроллер гема с эффектом
	/// </summary>
	public abstract class BaseGem<TEffect> : MonoBehaviour, IGem where TEffect : class, IEffect
	{
		protected TEffect _effect;

		public event Action<IGem> Collected;

		[Inject]
		private void Construct(IEffectFactory<TEffect> effectFactory)
		{
			_effect = effectFactory.Create();
		}

		private void OnTriggerEnter(Collider other)
		{
			CollectReaction();
		}

		/// <summary>
		/// Реакция на подбор бонус-гема
		/// </summary>
		public virtual void CollectReaction()
		{
			Collected?.Invoke(this);
			_effect.Apply();
		}

		public void Dispose()
		{
			if (this != null && gameObject != null)
			{
				Destroy(this.gameObject);
			}
		}
	}
}