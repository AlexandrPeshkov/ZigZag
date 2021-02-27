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

		public override void InstallBindings()
		{
			InstallSceneDependencies();
			BindServices();
		}

		/// <summary>
		/// Бинд сервисов
		/// </summary>
		private void BindServices()
		{
			Container.Bind<ScoreService>().AsSingle();

			Container.Bind<GameStateService>().FromNewComponentOnNewGameObject().AsSingle();

			Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();

			//MonoBehaviours
			Container.Bind<PlatformManager>().FromComponentInNewPrefab(_platformManagerPrefab).AsSingle().NonLazy();

			Container.Bind<SoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();

			Container.Bind<GameCameraController>().FromNewComponentOn(Camera.main.gameObject).AsSingle().NonLazy();

			Container.Bind<SphereController>().FromComponentInNewPrefab(_sphereControllerPrefab).AsSingle();

			Container.Bind<BonusManager>().FromComponentInNewPrefab(_bonusManagerPrefab).AsSingle().NonLazy();
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