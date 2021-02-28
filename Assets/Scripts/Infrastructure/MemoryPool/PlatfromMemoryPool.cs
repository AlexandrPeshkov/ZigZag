using Zenject;

namespace ZigZag.Infrastructure
{
	public class PlatfromMemoryPool : MonoPoolableMemoryPool<int, IMemoryPool, Platform>
	{
	}
}