using System;

namespace ZigZag
{
	/// <summary>
	/// Бонусный объект
	/// </summary>
	public interface IGem : IDisposable

	{
		/// <summary>
		/// Гем активирован
		/// </summary>
		event Action<IGem> Collected;

		/// <summary>
		/// Реакция на активацию
		/// </summary>
		void CollectReaction();
	}
}