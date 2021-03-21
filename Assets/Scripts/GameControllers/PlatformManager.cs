using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag
{
	public class PlatformManager : MonoBehaviour
	{
		private const int _basePointCost = 1;

		[SerializeField]
		private Platform _platformPrefab;

		private float _platformWidth;

		private Vector3 _top;

		private Vector3 _left;

		private List<Platform> _platforms;

		private GameStateService _gameStateService;

		private GameConfig _gameConfig;

		private Platform.Factory _platformFactory;

		private Platform _prevPlatform;

		private Platform _nextPlatform;

		public Platform CurrentPlatform { get; set; }

		public event Action<Platform> PlatformCreated;

		[Inject]
		private void Construct(GameStateService gameStateService, Platform.Factory platformFactory, GameConfig gameConfig)
		{
			_gameStateService = gameStateService;
			_platformFactory = platformFactory;
			_gameConfig = gameConfig;

			_gameStateService.GameStateChanged += OnGameStateChanged;

			_platformWidth = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * _platformPrefab.GetComponent<Transform>().localScale.x;

			_top = Vector3.forward * _platformWidth;
			_left = Vector3.left * _platformWidth;
		}

		private void Start()
		{
			Init();
		}

		/// <summary>
		/// Проверка положения сферы по границам платформ
		/// </summary>
		/// <param name="spherePos">Позиция в плоскости x-z</param>
		/// <returns></returns>
		public bool IsInPlatforms(Vector2 spherePos)
		{
			return (_prevPlatform?.TopRect.Contains(spherePos) == true)
			|| (CurrentPlatform?.TopRect.Contains(spherePos) == true)
			 || (_nextPlatform?.TopRect.Contains(spherePos) == true);
		}

		/// <summary>
		/// Расчет безопасного направления после восстановления
		/// </summary>
		public Vector3 GetSafetyDirection()
		{
			if (_nextPlatform != null && CurrentPlatform != null)
			{
				return (_nextPlatform.transform.position - CurrentPlatform.transform.position).normalized;
			}
			else
			{
				return Vector3.forward;
			}
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.StartNewGame:
					{
						OnNewGameStarted();
						break;
					}
				default: break;
			}
		}

		private void Init()
		{
			_platforms = new List<Platform>();
			CurrentPlatform = null;
			_prevPlatform = null;
			_nextPlatform = null;

			for (var i = 0; i < Math.Max(_gameConfig.PlatformPoolSize, _gameConfig.FirstLineLength); i++)
			{
				bool isFirstLine = i < _gameConfig.FirstLineLength;
				GeneratePlatform(_platforms.LastOrDefault(), isFirstLine);
			}
		}

		/// <summary>
		/// Создать платформу
		/// </summary>
		/// <param name="lastPlatform"></param>
		private void GeneratePlatform(Platform lastPlatform, bool isFirstLine = false)
		{
			Vector3 platformShift = UnityEngine.Random.Range(0, 2) == 0 ? _top : _left;

			Vector3 platformPos;
			if (isFirstLine)
			{
				platformPos = _top * _platforms.Count;
			}
			else
			{
				platformPos = lastPlatform._transform.position + platformShift;
			}

			Platform platform = _platformFactory.Create(_basePointCost, platformPos);

			platform.SphereOut += OnSphereOut;
			platform.SphereIn += OnSphereIn;

			_platforms.Add(platform);
			if (isFirstLine == false)
			{
				PlatformCreated?.Invoke(platform);
			}
		}

		/// <summary>
		/// Сфера покинула платформу
		/// </summary>
		/// <param name="outedPlatform">Платформа, которую прошла сфера</param>
		private void OnSphereOut(Platform outedPlatform)
		{
			int index = _platforms.IndexOf(outedPlatform);
			int fadeablePlatormIndex = index - _gameConfig.TailLengthForHide;

			//Скрыть платформу и вернуть в пул если на {TailLengthForHide} позади от текущей

			if (index > 0 && fadeablePlatormIndex > 0)
			{
				var fadeablePlatform = _platforms[fadeablePlatormIndex];

				fadeablePlatform.SphereOut -= OnSphereOut;
				fadeablePlatform.SphereIn -= OnSphereIn;
				_platforms.Remove(fadeablePlatform);

				fadeablePlatform.Dispose();
			}
			GeneratePlatform(_platforms.Last());
		}

		/// <summary>
		/// Сфера вошла на платформу
		/// </summary>
		/// <param name="platform">Активная платформа</param>
		private void OnSphereIn(Platform platform)
		{
			CurrentPlatform = platform;

			var index = _platforms.IndexOf(platform);

			if (index > -1)
			{
				var nextIndex = index + 1;
				if (nextIndex < _platforms.Count - 1)
				{
					_nextPlatform = _platforms[nextIndex];
				}

				var prevIndex = index - 1;
				if (prevIndex >= 0)
				{
					_prevPlatform = _platforms[prevIndex];
				}
			}
		}

		private void OnNewGameStarted()
		{
			foreach (var platform in _platforms)
			{
				platform.Dispose();
			}
			Init();
		}
	}
}