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
		private ScoreRecordText _newRecordMessage;

		[SerializeField]
		private ScoreTextView _scoreView;

		[SerializeField]
		private PauseView _pauseView;

		[SerializeField]
		private RecordTable _recordTable;

		[SerializeField]
		private Platform _platform;

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
			Container.Bind<ScoreService>().AsSingle();

			Container.Bind<GamePlayService>().AsSingle();

			//Container.Bind<GameStateService>().FromNewComponentOnNewGameObject().AsSingle();
			Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();

			Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();

			//MonoBehaviours
			Container.Bind<PlatformManager>().FromComponentInNewPrefab(_platformManagerPrefab).AsSingle().NonLazy();

			Container.Bind<SoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();

			Container.Bind<GameCameraController>().FromNewComponentOn(Camera.main.gameObject).AsSingle().NonLazy();

			Container.Bind<SphereController>().FromComponentInNewPrefab(_sphereControllerPrefab).AsSingle();

			Container.Bind<BonusManager>().FromComponentInNewPrefab(_bonusManagerPrefab).AsSingle().NonLazy();

			var config = Container.Resolve<GameConfig>();

			//Factories

			Container.BindFactory<int, Platform, Platform.Factory>()
				.FromMonoPoolableMemoryPool<int, Platform>(binder => binder
					.WithInitialSize(config.PlatformPoolSize)
					.FromComponentInNewPrefab(_platform)
					.UnderTransform(cntx => cntx.Container.Resolve<PlatformManager>().transform));

			Container.BindFactory<Platform, SpeedGem, SpeedGem, BonusGemFactory<SpeedGem>>().FromComponentInNewPrefab(_speedGemPrefab);
			Container.BindFactory<Platform, PointsGem, PointsGem, BonusGemFactory<PointsGem>>().FromComponentInNewPrefab(_pointsGemPrefab);

			//Container.Bind<IEffectFactory<SpeedEffect>>().To<SpeedEffectFactory>().AsSingle();
			//Container.Bind<IEffectFactory<PointsEffect>>().To<PointsEffectFactory>().AsSingle();

			Container.Bind<SpeedEffect>().FromFactory<SpeedEffectFactory>().AsTransient();

			Container.Bind<PointsEffect>().FromFactory<PointsEffectFactory>().AsTransient();
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
		}
	}
}