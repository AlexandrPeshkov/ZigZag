using UnityEngine;
using Zenject;
using ZigZag.UI;

namespace ZigZag
{
	/// <summary>
	/// Гем бонуса очков
	/// </summary>
	public class PointsGem : BaseGem<PointsEffect>
	{
		private DiContainer _container;

		[SerializeField]
		private FadedText _textPrefab;

		[Inject]
		private void Construct(DiContainer container)
		{
			_container = container;
		}

		public override void CollectReaction()
		{
			base.CollectReaction();

			FadedText text = _container.InstantiatePrefabForComponent<FadedText>(_textPrefab);
			text.Show(this.transform.position, $"+{_effect.Points}");
		}
	}
}