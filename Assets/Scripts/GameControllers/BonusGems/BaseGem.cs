using UnityEngine;
using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Базовый контроллер гема с эффектом
	/// </summary>
	public abstract class BaseGem<TEffect> : MonoBehaviour, IGem where TEffect : IEffect
	{
		protected TEffect _effect;

		public void BindEffect(IEffect effect)
		{
			_effect = effect;
		}

		private void OnCollisionEnter(Collision collision)
		{
			CollectReaction();
		}

		/// <summary>
		/// Реакция на подбор бонус-гема
		/// </summary>
		public virtual void CollectReaction()
		{
			_effect.Apply();
		}

		public void Dispose()
		{
			DestroyImmediate(this.gameObject);
		}
	}
}