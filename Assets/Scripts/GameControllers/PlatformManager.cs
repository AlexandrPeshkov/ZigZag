using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;

namespace ZigZag
{
	public class PlatformManager : MonoBehaviour
	{
		private const int _platformCapacity = 15;

		[SerializeField]
		private Transform _platformParent;

		[SerializeField]
		private Platform _platformPrefab;

		private LinkedList<Platform> _platforms;

		private float _platformSize;

		private Vector3 _top;

		private Vector3 _left;

		private SignalBus _signalBus;

		[Inject]
		private void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;

			_signalBus.Subscribe<GameStateSignal>(OnGameStateChanged);

			_platformSize = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * _platformPrefab.GetComponent<Transform>().localScale.x;

			_top = Vector3.forward * _platformSize;
			_left = Vector3.left * _platformSize;
			Init();
		}

		private void OnGameStateChanged(GameStateSignal signal)
		{
			switch (signal.GameState)
			{
				case GameState.Reset:
					{
						OnGameReset();
						break;
					}
				default: break;
			}
		}

		private void Init()
		{
			_platforms = new LinkedList<Platform>();
			FirstLinePlatforms();

			for (var i = 1; i < _platformCapacity; i++)
			{
				GeneratePlatform(_platforms.Last.Value);
			}
		}

		private void FirstLinePlatforms()
		{
			for (var i = 0; i < 4; i++)
			{
				Platform platform = Instantiate(_platformPrefab, _platformParent, true);
				platform._transform.position = Vector3.zero + _top * i;
				var node = _platforms.AddLast(platform);
				platform.SpehreIsOut += OnSphereOut;
				platform.LinkPlatform(node);
			}
		}

		private void GeneratePlatform(Platform lastPlatform)
		{
			Platform platform = Instantiate(_platformPrefab, _platformParent, true);

			Vector3 platformShift = UnityEngine.Random.Range(0, 2) == 0 ? _top : _left;

			platform._transform.position = lastPlatform._transform.position + platformShift;

			platform.SpehreIsOut += OnSphereOut;

			var node = _platforms.AddLast(platform);
			platform.LinkPlatform(node);
		}

		/// <summary>
		/// Сфера покинула платформу
		/// </summary>
		/// <param name="lastPlatform"></param>
		private void OnSphereOut(Platform lastPlatform)
		{
			_signalBus.Fire(new PlatformCompleteSignal(1));

			Platform previousPlatform = lastPlatform.Node.Previous?.Previous?.Value;

			if (previousPlatform != null && previousPlatform != lastPlatform)
			{
				previousPlatform.SpehreIsOut -= OnSphereOut;

				_platforms.Remove(previousPlatform);
				previousPlatform.Fall();

				GeneratePlatform(_platforms.Last.Value);
			}
		}

		private void OnGameReset()
		{
			foreach (var platform in _platforms)
			{
				DestroyImmediate(platform.gameObject);
			}
			Init();
		}
	}
}