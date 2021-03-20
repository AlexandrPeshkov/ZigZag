using ZigZag.UI;

namespace ZigZag
{
	/// <summary>
	/// Гем - здоровья
	/// </summary>
	public class LifeGem : BaseGem<LifeEffect>
	{
		protected override void ApplyEffect()
		{
			LifeEffect effect = _effectManager.ApplyEffect<LifeEffect>();

			FadeableHint hint = _container.InstantiatePrefabForComponent<FadeableHint>(_reactionHint);
			hint.SetText($"+{effect.LifesCount}");
			hint.Show(this.transform.position);
		}
	}
}