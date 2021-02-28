﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;
using Random = UnityEngine.Random;

namespace ZigZag
{
	/// <summary>
	/// Управление бонусами
	/// </summary>
	public class BonusManager : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> _gemPrefabs;

		[SerializeField]
		private Platform _platformPrefab;

		private PlatformManager _platformManager;

		private GameConfig _gameConfig;

		private GameStateService _gameStateService;

		private DiContainer _container;

		private List<IGem> _spawnedGems;

		private Dictionary<Type, IFactory> _factoriesCache;

		[SerializeField]
		private Transform _gemParent;

		[Inject]
		private void Construct(PlatformManager platformManager,
			GameConfig gameConfig,
			GameStateService gameStateService,
			DiContainer container)
		{
			_platformManager = platformManager;
			_gameConfig = gameConfig;
			_gameStateService = gameStateService;
			_container = container;

			_spawnedGems = new List<IGem>();
			_factoriesCache = new Dictionary<Type, IFactory>();

			_gameStateService.GameStateChanged += OnGameStateChanged;
			_platformManager.PlatformCreated += OnPlatformCreated;
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Reset:
					{
						ClearBonuses();
						break;
					}
			}
		}

		private void OnPlatformCreated(Platform platform)
		{
			List<Action<Platform>> spawnActions = new List<Action<Platform>>();

			if (Random.Range(0, 1f) >= (1f - _gameConfig.PointsGemChance))
			{
				spawnActions.Add(SpawnGem<PointsGem>);
			}
			if (Random.Range(0, 1f) >= (1f - _gameConfig.SpeedGemChance))
			{
				spawnActions.Add(SpawnGem<SpeedGem>);
			}

			if (spawnActions.Count > 0)
			{
				int actionIndex = Random.Range(0, spawnActions.Count);

				spawnActions[actionIndex](platform);
			}
		}

		/// <summary>
		/// Создать бонусный гем
		/// </summary>
		/// <typeparam name="TGem">Тип гема</typeparam>
		/// <param name="platform">Платформа на которой будет создан гем</param>
		private void SpawnGem<TGem>(Platform platform) where TGem : Component, IGem
		{
			Type factoryType = typeof(BonusGemFactory<TGem>);
			BonusGemFactory<TGem> factory = null;
			if (_factoriesCache.TryGetValue(factoryType, out var cachedFactory) == false)
			{
				factory = _container.Resolve<BonusGemFactory<TGem>>();
				_factoriesCache.Add(factoryType, factory);
			}
			else
			{
				factory = cachedFactory as BonusGemFactory<TGem>;
			}

			TGem gemPrefab = _gemPrefabs.Select(x => x.GetComponent<TGem>()).FirstOrDefault(x => x != null);

			if (gemPrefab != null && factory != null)
			{
				TGem gem = factory.Create(platform);
				gem.transform.SetParent(_gemParent);
				gem.Collected += OnGemCollected;
				_spawnedGems.Add(gem);
			}
			else
			{
				Debug.LogError($"Не найдена фабрика или префаб для типа {typeof(TGem).Name}");
			}
		}

		private void OnGemCollected(IGem gem)
		{
			_spawnedGems.Remove(gem);
			gem.Dispose();
		}

		private void ClearBonuses()
		{
			foreach (var bonus in _spawnedGems)
			{
				bonus.Dispose();
			}
		}
	}
}