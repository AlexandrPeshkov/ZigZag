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

		protected IMemoryPool _memoryPool;

		private Platform _bindedPlatform;

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

		public void OnSpawned(IMemoryPool pool, Platform platform)
		{
			if (_bindedPlatform != null)
			{
				_bindedPlatform.Disposed -= OnPlatformDisposed;
			}
			_bindedPlatform = platform;

			_bindedPlatform.Disposed += OnPlatformDisposed;
			_memoryPool = pool;
		}

		private void OnPlatformDisposed(Platform platform)
		{
			_bindedPlatform.Disposed -= OnPlatformDisposed;
			_bindedPlatform = null;
			Dispose();
		}

		public void OnDespawned()
		{
			_memoryPool = null;
		}

		public void Dispose()
		{
			_memoryPool?.Despawn(this);
		}
	}
}