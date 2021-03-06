using UnityEngine;
using Zenject;

namespace ZigZag.Infrastructure
{
	public class PlatfromMemoryPool : MonoPoolableMemoryPool<int, Vector3, IMemoryPool, Platform>
	{
	}
}