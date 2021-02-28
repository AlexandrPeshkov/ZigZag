using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZigZag.Services;
using ZigZag.UI;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// DI установщик
	/// </summary>
	public class SceneInstaller : MonoInstaller
	{
		[Header("Prefabs")]
		[SerializeField]
		private PlatformManager _platformManagerPrefab;

		[SerializeField]
		private SoundManager _soundManagerPrefab;

		[SerializeField]
		private SphereController _sphereControllerPrefab;

		[SerializeField]
		private BonusManager _bonusManagerPrefab;

		[Header("Gems")]
		[SerializeField]
		private SpeedGem _speedGemPrefab;

		[SerializeField]
		private PointsGem _pointsGemPrefab;

		[Header("Scene instans")]
		[SerializeField]
		private FinishGameDialog _finishGameDialog;

		[SerializeField]
		private NewRecordText _newRecordMessage;

		[SerializeField]
		private ScoreValueText _scoreView;

		[SerializeField]
		private PauseView _pauseView;

		[SerializeField]
		private RecordTable _recordTable;

		[SerializeField]
		private Platform _platform;

		[SerializeField]
		private Camera _mainCamera;

		[SerializeField]
		private Canvas _mainCanvas;

		[Inject]
		private GameConfig _gameConfig;

		public override void InstallBindings()
		{
			Application.targetFrameRate = 1000;
			InstallSceneDependencies();
			BindServices();
		}

		/// <summary>
		/// Бинд сервисов
		/// </summary>
		private void BindServices()
		{
			//Services
			Container.Bind<ScoreService>().AsSingle();

			Container.Bind<GamePlayService>().AsSingle();

			Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();

			Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();

			//Game managers
			Container.Bind<PlatformManager>().FromComponentInNewPrefab(_platformManagerPrefab).AsSingle().NonLazy();

			Container.Bind<SoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();

			Container.Bind<GameCameraController>().FromNewComponentOn(Camera.main.gameObject).AsSingle().NonLazy();

			Container.Bind<SphereController>().FromComponentInNewPrefab(_sphereControllerPrefab).AsSingle();

			Container.Bind<BonusManager>().FromComponentInNewPrefab(_bonusManagerPrefab).AsSingle().NonLazy();

			//Memory pool

			Container.BindFactory<int, Platform, Platform.Factory>()
				.FromMonoPoolableMemoryPool<int, Platform>(binder => binder
					.WithInitialSize(_gameConfig.PlatformPoolSize)
					.FromComponentInNewPrefab(_platform)
					.UnderTransform(cntx => cntx.Container.Resolve<PlatformManager>().transform));

			//Bonus gems
			Container.BindFactory<Platform, SpeedGem, BonusGemFactory<SpeedGem>>()
				.WithFactoryArgumentsExplicit(new TypeValuePair[] { new TypeValuePair { Type = typeof(SpeedGem), Value = _speedGemPrefab } })
				.FromComponentInNewPrefab(_speedGemPrefab);

			Container.BindFactory<Platform, PointsGem, BonusGemFactory<PointsGem>>()
				.WithFactoryArgumentsExplicit(new TypeValuePair[] { new TypeValuePair { Type = typeof(PointsGem), Value = _pointsGemPrefab } })
				.FromComponentInNewPrefab(_speedGemPrefab);

			Container.Bind<IEffectFactory<SpeedEffect>>().To<SpeedEffectFactory>().AsSingle();
			Container.Bind<IEffectFactory<PointsEffect>>().To<PointsEffectFactory>().AsSingle();

			//UI
		}

		/// <summary>
		/// Бинд зависимсотей со сцены
		/// </summary>
		private void InstallSceneDependencies()
		{
			Container.BindInstance(_finishGameDialog).AsSingle();

			Container.BindInstance(_newRecordMessage).AsSingle();

			Container.BindInstance(_scoreView).AsSingle();

			Container.BindInstance(_pauseView).AsSingle();

			Container.BindInstance(_recordTable).AsSingle();

			Container.BindInstance(_mainCamera).WithId(DiConstants._mainCamera).AsSingle();

			Container.BindInstance(_mainCanvas).WithId(DiConstants._mainCanvas).AsSingle();
		}
	}
}