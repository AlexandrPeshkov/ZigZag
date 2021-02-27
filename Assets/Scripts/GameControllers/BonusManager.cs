using UnityEngine;
using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Управление бонусами
	/// </summary>
	public class BonusManager : MonoBehaviour
	{
		[SerializeField]
		private Bonus _pointBonusPrefab;

		[SerializeField]
		private Platform _platformPrefab;

		private Vector3 _pointBonusOffset;

		private PlatformManager _platformManager;

		[Inject]
		private void Construct(PlatformManager platformManager)
		{
			_platformManager = platformManager;
			_platformManager.PlatformCreated += OnPlatformCreated;

			var pointBonusY = _pointBonusPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * _pointBonusPrefab.GetComponent<Transform>().localScale.y;
			var platformY = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * _platformPrefab.GetComponent<Transform>().localScale.y * 0.5f;

			_pointBonusOffset = new Vector3(0, pointBonusY + platformY, 0);
		}

		private void OnPlatformCreated(Platform platform)
		{
		}

		private Bonus GenereateBonusOnPlatform(Platform platform)
		{
			Bonus bonus = Instantiate(_pointBonusPrefab, this.transform, false);
			bonus.transform.position = platform.transform.position + _pointBonusOffset;

			return bonus;
		}
	}
}