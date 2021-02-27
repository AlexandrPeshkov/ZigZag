using System;
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
		public List<IGem> _gemPrefabs2;

		[SerializeField]
		private List<GameObject> _gemPrefabs;

		[SerializeField]
		private Platform _platformPrefab;

		private PlatformManager _platformManager;

		private GameConfig _gameConfig;

		private GameStateService _gameStateService;

		private DiContainer _container;

		private List<IGem> _spawnedGems;

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

			if (Random.Range(0, 1.01f) >= _gameConfig.PointsBonusProbability)
			{
				spawnActions.Add(SpawnGem<PointsGem>);
			}
			if (Random.Range(0, 1.01f) >= _gameConfig.SpeedBonusProbability)
			{
				spawnActions.Add(SpawnGem<SpeedGem>);
			}

			int actionIndex = Random.Range(0, spawnActions.Count);
			spawnActions[actionIndex](platform);
		}

		private void SpawnGem<TGem>(Platform platform) where TGem : Component, IGem
		{
			var factory = _container.Resolve<BonusGemFactory<TGem>>();

			var gemPrefab = _gemPrefabs.Select(x => x.GetComponent<TGem>()).FirstOrDefault(x => x != null);

			if (gemPrefab != null && factory != null)
			{
				TGem gem = factory.Create(platform, gemPrefab);
				gem.transform.SetParent(this.transform);

				_spawnedGems.Add(gem);
			}
			else
			{
				Debug.LogError($"Не найдена фабрика или префаб для типа {typeof(TGem).Name}");
			}
		}

		private void ClearBonuses()
		{
			foreach (var bonus in _spawnedGems)
			{
				bonus.Dispose();
			}
		}
	}

	public class Factory : PlaceholderFactory<IGem> { }
}