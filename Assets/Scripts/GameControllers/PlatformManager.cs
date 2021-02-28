﻿using System;
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
			_platforms = new List<Platform>();
			FirstLinePlatforms();
			for (var i = _gameConfig.firstLineLength; i < _gameConfig.PlatformPoolSize; i++)
			{
				GeneratePlatform(_platforms.Last()); ;
			}
		}

		/// <summary>
		/// Прямая линия начальныъх платформ
		/// </summary>
		private void FirstLinePlatforms()
		{
			for (var i = 0; i < _gameConfig.firstLineLength; i++)
			{
				Platform platform = _platformFactory.Create(_basePointCost);

				platform._transform.position = Vector3.zero + _top * i;

				_platforms.Add(platform);
				platform.SpehreIsOut += OnSphereOut;
			}
		}

		/// <summary>
		/// Создать платформу
		/// </summary>
		/// <param name="lastPlatform"></param>
		private void GeneratePlatform(Platform lastPlatform)
		{
			Platform platform = _platformFactory.Create(_basePointCost);

			Vector3 platformShift = UnityEngine.Random.Range(0, 2) == 0 ? _top : _left;

			platform._transform.position = lastPlatform._transform.position + platformShift;

			platform.SpehreIsOut += OnSphereOut;

			_platforms.Add(platform);

			PlatformCreated?.Invoke(platform);
		}

		/// <summary>
		/// Сфера покинула платформу
		/// </summary>
		/// <param name="outedPlatform">Платформа, которую прошла сфера</param>
		private void OnSphereOut(Platform outedPlatform)
		{
			int tailStep = 15;

			int index = _platforms.IndexOf(outedPlatform);
			int fadeablePlatormIndex = index - tailStep;

			//Скрыть платформу и вернуть в пул на {tailStep} позади от текущекй

			if (index > 0 && fadeablePlatormIndex > 0)
			{
				var fadeablePlatform = _platforms[fadeablePlatormIndex];

				fadeablePlatform.SpehreIsOut -= OnSphereOut;

				_platforms.Remove(fadeablePlatform);

				fadeablePlatform.Dispose();
			}
			GeneratePlatform(_platforms.Last());
		}

		private void OnGameReset()
		{
			foreach (var platform in _platforms)
			{
				platform.Dispose();
				//DestroyImmediate(platform.gameObject);
			}
			Init();
		}
	}
}