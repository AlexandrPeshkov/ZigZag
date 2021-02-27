using System;

namespace ZigZag
{
	/// <summary>
	/// Бонусный объект
	/// </summary>
	public interface IGem : IDisposable

	{
		void CollectReaction();

		void BindEffect(IEffect effect);
	}
}