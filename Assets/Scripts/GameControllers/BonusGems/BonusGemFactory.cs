using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class BonusGemFactory<TGem> : PlaceholderFactory<Platform, TGem>
		where TGem : Component, IGem
	{
		private DiContainer _container;

		private TGem _gemPrefab;

		[Inject]
		private void Construct(DiContainer container, TGem gemPrefab)
		{
			_container = container;
			_gemPrefab = gemPrefab;
		}

		public override TGem Create(Platform platform)
		{
			try
			{
				TGem gem = _container.InstantiatePrefabForComponent<TGem>(_gemPrefab);

				var platformY = platform.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * platform.GetComponent<Transform>().localScale.y * 0.5f;

				var pointBonusY = _gemPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * _gemPrefab.GetComponent<Transform>().localScale.y;
				var gemOffset = new Vector3(0, pointBonusY + platformY, 0);

				gem.transform.position = platform.transform.position + gemOffset;

				return gem;
			}
			catch (System.Exception ex)
			{
			}
			return null;
		}
	}
}