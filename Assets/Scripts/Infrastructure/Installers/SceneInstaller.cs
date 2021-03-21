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
		[Header("Domain prefabs")]
		[SerializeField]
		private PlatformManager _platformManagerPrefab;

		[SerializeField]
		private SoundManager _soundManagerPrefab;

		[SerializeField]
		private SphereController _sphereControllerPrefab;

		[SerializeField]
		private GemManager _bonusManagerPrefab;

		[SerializeField]
		private Platform _platform;

		[Header("Gems")]
		[SerializeField]
		private SpeedGem _speedGemPrefab;

		[SerializeField]
		private PointsGem _pointsGemPrefab;

		[SerializeField]
		private LifeGem _lifeGemPrefab;

		[Header("Scene instans")]
		[SerializeField]
		private FinishGameDialog _finishGameDialog;

		[SerializeField]
		private NewRecordText _newRecordMessage;

		[SerializeField]
		private ScoreValueView _scoreView;

		[SerializeField]
		private LifeValueView _lifesView;

		[SerializeField]
		private PauseView _pauseView;

		[SerializeField]
		private RecordTable _recordTable;

		[SerializeField]
		private Canvas _mainCanvas;

		[Inject]
		private GameConfig _gameConfig;

		public override void InstallBindings()
		{
			InstallSceneDependencies();
			BindServices();
			InstallMemoryPools();
		}

		/// <summary>
		/// Бинд сервисов
		/// </summary>
		private void BindServices()
		{
			//Services
			Container.Bind<ScoreService>().AsSingle();

			Container.Bind<GamePlayService>().AsSingle();

			Container.Bind<PlayerPrefsStorage>().AsSingle();

			Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();

			Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();

			Container.Bind<CoroutineService>().AsSingle();

			//Game managers
			Container.Bind<PlatformManager>().FromComponentInNewPrefab(_platformManagerPrefab).AsSingle().NonLazy();

			Container.Bind<SoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();

			Container.Bind<SphereController>()
				.FromComponentInNewPrefab(_sphereControllerPrefab)
				.WithGameObjectName(SphereController._objectName)
				.AsSingle()
				.NonLazy();

			Container.Bind<Camera>()
				.WithId(DiConstants._mainCamera)
				.FromComponentOn(cntx => cntx.Container.Resolve<SphereController>().gameObject.GetComponentInChildren<Camera>().gameObject)
				.AsSingle();

			Container.Bind<GemManager>().FromComponentInNewPrefab(_bonusManagerPrefab).AsSingle().NonLazy();

			//Gems and effects
			Container.Bind<EffectManager>().AsSingle();

			Container.Bind<SpeedEffect>().AsTransient();
			Container.Bind<PointsEffect>().AsTransient();
			Container.Bind<LifeEffect>().AsTransient();

			Container.Bind<IEffectFactory<SpeedEffect>>().To<SpeedEffectFactory>().AsSingle();
			Container.Bind<IEffectFactory<PointsEffect>>().To<PointsEffectFactory>().AsSingle();
			Container.Bind<IEffectFactory<LifeEffect>>().To<LifeEffectFactory>().AsSingle();

			Container.BindFactory<Platform, PointsGem, GemFactory<PointsGem>>()
				.WithFactoryArgumentsExplicit(new TypeValuePair[] { new TypeValuePair { Type = typeof(PointsGem), Value = _pointsGemPrefab } })
				.FromComponentInNewPrefab(_pointsGemPrefab);

			Container.BindFactory<Platform, SpeedGem, GemFactory<SpeedGem>>()
				.WithFactoryArgumentsExplicit(new TypeValuePair[] { new TypeValuePair { Type = typeof(SpeedGem), Value = _speedGemPrefab } })
				.FromComponentInNewPrefab(_speedGemPrefab);

			Container.BindFactory<Platform, LifeGem, GemFactory<LifeGem>>()
				.WithFactoryArgumentsExplicit(new TypeValuePair[] { new TypeValuePair { Type = typeof(LifeGem), Value = _lifeGemPrefab } })
				.FromComponentInNewPrefab(_lifeGemPrefab);
		}

		/// <summary>
		/// Бинд зависимсотей со сцены
		/// </summary>
		private void InstallSceneDependencies()
		{
			Container.BindInstance(_finishGameDialog).AsSingle();

			Container.BindInstance(_newRecordMessage).AsSingle();

			Container.BindInstance(_scoreView).AsSingle();

			Container.BindInstance(_lifesView).AsSingle();

			Container.BindInstance(_pauseView).AsSingle();

			Container.BindInstance(_recordTable).AsSingle();

			Container.BindInstance(_mainCanvas).WithId(DiConstants._mainCanvas).AsSingle();
		}

		/// <summary>
		/// Бинд пулов
		/// </summary>
		private void InstallMemoryPools()
		{
			Container.BindFactory<int, Vector3, Platform, Platform.Factory>()
				.FromMonoPoolableMemoryPool<int, Vector3, Platform>(binder => binder
					.WithInitialSize(_gameConfig.FirstLineLength + _gameConfig.TailLengthForHide)
					.FromComponentInNewPrefab(_platform)
					.UnderTransform(cntx => cntx.Container.Resolve<PlatformManager>().transform));

			Container.BindMemoryPool<PointsGem, GemMemoryPool<PointsGem>>()
				.WithInitialSize(_gameConfig.GemPoolSize)
				.FromComponentInNewPrefab(_pointsGemPrefab)
				.UnderTransform(cntx => cntx.Container.Resolve<GemManager>().transform);

			Container.BindMemoryPool<SpeedGem, GemMemoryPool<SpeedGem>>()
				.WithInitialSize(_gameConfig.GemPoolSize)
				.FromComponentInNewPrefab(_speedGemPrefab)
				.UnderTransform(cntx => cntx.Container.Resolve<GemManager>().transform);

			Container.BindMemoryPool<LifeGem, GemMemoryPool<LifeGem>>()
				.WithInitialSize(_gameConfig.GemPoolSize)
				.FromComponentInNewPrefab(_lifeGemPrefab)
				.UnderTransform(cntx => cntx.Container.Resolve<GemManager>().transform);
		}
	}
}