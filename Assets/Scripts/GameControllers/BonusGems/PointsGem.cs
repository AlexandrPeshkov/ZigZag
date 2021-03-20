using ZigZag.UI;

namespace ZigZag
{
	/// <summary>
	/// Гем бонуса очков
	/// </summary>
	public class PointsGem : BaseGem<PointsEffect>
	{
		protected override void ApplyEffect()
		{
			PointsEffect effect = _effectManager.ApplyEffect<PointsEffect>();

			FadeableText hint = _container.InstantiatePrefabForComponent<FadeableText>(_reactionHint);
			hint.SetText($"+{effect.Points}");
			hint.Show(this.transform.position);
		}
	}
}