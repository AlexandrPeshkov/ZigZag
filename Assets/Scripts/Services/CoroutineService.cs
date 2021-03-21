using System.Collections;
using UnityEngine;

namespace ZigZag.Services
{
	public sealed class CoroutineHolder : MonoBehaviour
	{
	}

	public class CoroutineService
	{
		private CoroutineHolder _runner;

		private readonly object _lock = new object();

		private CoroutineHolder Runner
		{
			get
			{
				if (_runner == null)
				{
					lock (_lock)
					{
						if (_runner == null)
						{
							_runner = new GameObject("Static Coroutine RestClient").AddComponent<CoroutineHolder>();
							Object.DontDestroyOnLoad(_runner);
						}
					}
				}

				return _runner;
			}
		}

		public Coroutine StartCoroutine(IEnumerator coroutine) => Runner.StartCoroutine(coroutine);

		public void StopCoroutine(Coroutine coroutine)
		{
			if (coroutine != null)
			{
				Runner.StopCoroutine(coroutine);
			}
		}

		public void Dispose()
		{
			Object.DestroyImmediate(_runner);
		}
	}
}