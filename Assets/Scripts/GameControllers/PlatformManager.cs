using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag
{
	public class PlatformManager : MonoBehaviour
	{
		private const int _platformCapacity = 25;

		[SerializeField]
		private Transform _platformParent;

		[SerializeField]
		private Platform _platformPrefab;

		private float _platformWidth;

		private Vector3 _top;

		private Vector3 _left;

		private LinkedList<Platform> _platforms;

		private GameStateService _gameStateService;

		public event Action<Platform> PlatformCreated;

		public event Action<Platform> PlatformPassed;

		[Inject]
		private void Construct(GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
			_gameStateService.GameStateChanged += OnGameStateChanged;

			_platformWidth = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * _platformPrefab.GetComponent<Transform>().localScale.x;

			_top = Vector3.forward * _platformWidth;
			_left = Vector3.left * _platformWidth;
			Init();
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
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
				platform.InitPlatform(node, 1);
			}
		}

		private void GeneratePlatform(Platform lastPlatform)
		{
			Platform platform = Instantiate(_platformPrefab, _platformParent, true);

			Vector3 platformShift = UnityEngine.Random.Range(0, 2) == 0 ? _top : _left;

			platform._transform.position = lastPlatform._transform.position + platformShift;

			platform.SpehreIsOut += OnSphereOut;
			platform.SphereIn += OnSphereIn;

			var node = _platforms.AddLast(platform);
			platform.InitPlatform(node, 1);

			PlatformCreated?.Invoke(platform);
		}

		/// <summary>
		/// Сфера покинула платформу
		/// </summary>
		/// <param name="platform">Платформа, которую прошла сфера</param>
		private void OnSphereOut(Platform platform)
		{
			Platform previousPlatform = platform.Node.Previous?.Previous?.Value;

			if (previousPlatform != null && previousPlatform != platform)
			{
				previousPlatform.SpehreIsOut -= OnSphereOut;
				previousPlatform.SphereIn -= OnSphereIn;

				_platforms.Remove(previousPlatform);
				previousPlatform.Fall();

				GeneratePlatform(_platforms.Last.Value);
			}
		}

		/// <summary>
		/// Сфера покинула платформу
		/// </summary>
		/// <param name="platform">Платформа на которую вошла сфера</param>
		private void OnSphereIn(Platform platform)
		{
			PlatformPassed?.Invoke(platform);
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