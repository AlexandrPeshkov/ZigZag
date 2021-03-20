using System;
using UnityEngine;
using Zenject;
using ZigZag.Infrastructure;
using ZigZag.UI;

namespace ZigZag
{
	/// <summary>
	/// Базовый контроллер гема с эффектом
	/// </summary>
	public abstract class BaseGem<TEffect> : MonoBehaviour, IGem where TEffect : class, IEffect
	{
		/// <summary>
		/// Звук активации гема
		/// </summary>
		[SerializeField]
		protected AudioClip _sound;

		/// <summary>
		/// Префаб хинта эффекта при активации гема
		/// </summary>
		[SerializeField]
		protected BaseFadeableElem _reactionHint;

		protected SoundManager _soundManager;

		protected EffectManager _effectManager;

		protected DiContainer _container;

		public event Action<IGem> Collected;

		[Inject]
		private void Construct(EffectManager effectManager, SoundManager soundManager, DiContainer container)
		{
			_effectManager = effectManager;
			_soundManager = soundManager;
			_container = container;
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
		public void CollectReaction()
		{
			PlaySound();
			ApplyEffect();
			Collected?.Invoke(this);
		}

		protected virtual void PlaySound()
		{
			_soundManager.PlayEffectSound(_sound);
		}

		protected abstract void ApplyEffect();

		public void Dispose()
		{
			if (this != null && gameObject != null)
			{
				Destroy(this.gameObject);
			}
		}
	}
}