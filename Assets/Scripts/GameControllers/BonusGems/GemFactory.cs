using UnityEngine;
using Zenject;
using ZigZag.Infrastructure;

namespace ZigZag
{
	public class GemFactory<TGem> : PlaceholderFactory<Platform, TGem>
		where TGem : Component, IGem
	{
		private GemMemoryPool<TGem> _gemPool;

		private TGem _gemPrefab;

		[Inject]
		private void Construct(TGem gemPrefab, GemMemoryPool<TGem> gemPool)
		{
			_gemPrefab = gemPrefab;
			_gemPool = gemPool;
		}

		public override TGem Create(Platform platform)
		{
			TGem gem = _gemPool.Spawn(_gemPool, platform);

			var platformHeight = platform.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * platform.GetComponent<Transform>().localScale.y * 0.5f;

			var gemHeight = _gemPrefab.GetComponent<MeshRenderer>().bounds.size.y * _gemPrefab.GetComponent<Transform>().localScale.y;

			var gemOffset = new Vector3(0, gemHeight + platformHeight, 0);

			gem.transform.position = platform.transform.position + gemOffset;

			return gem;
		}
	}
}