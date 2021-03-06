using System;
using UnityEngine;
using Zenject;
using ZigZag.Infrastructure;

namespace ZigZag
{
	/// <summary>
	/// Базовый контроллер гема с эффектом
	/// </summary>
	public abstract class BaseGem<TEffect> : MonoBehaviour, IGem where TEffect : class, IEffect
	{
		[SerializeField]
		private AudioClip _sound;

		private SoundManager _soundManager;

		protected TEffect _effect;

		public event Action<IGem> Collected;

		[Inject]
		private void Construct(IEffectFactory<TEffect> effectFactory, SoundManager soundManager)
		{
			_effect = effectFactory.Create();
			_soundManager = soundManager;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name == SphereController._objectName)
			{
				CollectReaction();
			}
		}

		/// <summary>
		/// Реакция на подбор бонус-гема
		/// </summary>
		public virtual void CollectReaction()
		{
			Collected?.Invoke(this);
			_effect.Apply();
			_soundManager.PlayEffectSound(_sound);
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