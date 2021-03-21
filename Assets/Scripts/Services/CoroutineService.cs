using System.Collections;
using UnityEngine;

namespace ZigZag.Services
{
	/// <summary>
	/// Сервис работы с коротинами
	/// </summary>
	public class CoroutineService
	{
		private CoroutineHolder _holder;

		private readonly object _lock = new object();

		private CoroutineHolder Holder
		{
			get
			{
				if (_holder == null)
				{
					lock (_lock)
					{
						if (_holder == null)
						{
							_holder = new GameObject("Static Coroutine RestClient").AddComponent<CoroutineHolder>();
							Object.DontDestroyOnLoad(_holder);
						}
					}
				}

				return _holder;
			}
		}

		/// <summary>
		/// Запустить коротину
		/// </summary>
		/// <param name="coroutine"></param>
		/// <returns></returns>
		public Coroutine StartCoroutine(IEnumerator coroutine) => Holder.StartCoroutine(coroutine);

		/// <summary>
		/// Остановить коротину
		/// </summary>
		/// <param name="coroutine"></param>
		public void StopCoroutine(Coroutine coroutine)
		{
			if (coroutine != null)
			{
				Holder.StopCoroutine(coroutine);
			}
		}

		public sealed class CoroutineHolder : MonoBehaviour { }
	}
}