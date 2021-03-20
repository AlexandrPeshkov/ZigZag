using System;
using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Бонусный объект
	/// </summary>
	public interface IGem : IPoolable<IMemoryPool, Platform>, IDisposable
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