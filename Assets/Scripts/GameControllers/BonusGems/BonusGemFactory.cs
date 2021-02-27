using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class BonusGemFactory<TGem, TEffect> : PlaceholderFactory<Platform, TGem, TGem>
		where TGem : Component, IGem
		where TEffect : IEffect
	{
		private DiContainer _container;

		[Inject]
		public void Construct(DiContainer container)
		{
			_container = container;
		}

		//TODO Фабрика не создает автоматически внутри себя эффект (
		public override TGem Create(Platform platform, TGem gemPrefab)
		{
			TGem gem = base.Create(platform, gemPrefab);
			var effectFactory = _container.Resolve<IEffectFactory<TEffect>>();

			var platformY = platform.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * platform.GetComponent<Transform>().localScale.y * 0.5f;

			var pointBonusY = gemPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * gemPrefab.GetComponent<Transform>().localScale.y;
			var gemOffset = new Vector3(0, pointBonusY + platformY, 0);

			gem.transform.position = platform.transform.position + gemOffset;

			return gem;
		}
	}
}