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
		[SerializeField]
		private AudioClip _sound;

		/// <summary>
		/// Префаб текста эффекта при активации гема
		/// </summary>
		[SerializeField]
		protected FadedText _textPrefab;

		private SoundManager _soundManager;

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
		public virtual void CollectReaction()
		{
			Collected?.Invoke(this);

			TEffect effect = _effectManager.ApplyEffect<TEffect>();
			_soundManager.PlayEffectSound(_sound);

			FadedText text = _container.InstantiatePrefabForComponent<FadedText>(_textPrefab);

			text.Show(this.transform.position, $"+{effect.Text}");
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