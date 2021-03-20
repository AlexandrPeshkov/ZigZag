using UnityEngine;
using Zenject;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Пул гемов
	/// </summary>
	/// <typeparam name="TGem"></typeparam>
	public class GemMemoryPool<TGem> : MonoPoolableMemoryPool<IMemoryPool, Platform, TGem>
		where TGem : Component, IGem
	{
	}
}