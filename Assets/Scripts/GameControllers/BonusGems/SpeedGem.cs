using UnityEngine;
using ZigZag.UI;

namespace ZigZag
{
	/// <summary>
	/// Гем онуса скорости
	/// </summary>
	public class SpeedGem : BaseGem<SpeedEffect>
	{
		[SerializeField]
		private string _activationText = "Speed up!";

		protected override void ApplyEffect()
		{
			_effectManager.ApplyEffect<SpeedEffect>();

			FadeableHint hint = _container.InstantiatePrefabForComponent<FadeableHint>(_reactionHint);
			hint.SetText(_activationText);
			hint.Show(this.transform.position);
		}
	}
}