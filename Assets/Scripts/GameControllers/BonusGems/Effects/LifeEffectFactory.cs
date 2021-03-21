using Zenject;

namespace ZigZag
{
	public class LifeEffectFactory : IEffectFactory<LifeEffect>
	{
		private readonly DiContainer _container;

		public LifeEffectFactory(DiContainer container)
		{
			_container = container;
		}

		public LifeEffect Create()
		{
			LifeEffect lifeEffect = _container.Resolve<LifeEffect>();

			lifeEffect.Initialize(lifes: 1);

			return lifeEffect;
		}
	}
}