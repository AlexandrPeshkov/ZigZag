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
		[SerializeField]
		private PlatformManager _platformManager;

		[SerializeField]
		private SphereController _sphereController;

		[SerializeField]
		private FinishGameDialog _finishGamePanel;

		[SerializeField]
		private NewRecordLabel _newRecordMessage;

		[SerializeField]
		private ScoreTextView _scoreView;

		public override void InstallBindings()
		{
			InstallSignals();
			InstallSceneDependencies();
			BindServices();
		}

		/// <summary>
		/// Бинд сервисов
		/// </summary>
		private void BindServices()
		{
			Container.Bind<ScoreService>().AsSingle();
			Container.Bind<GameStateService>().AsSingle();
		}

		/// <summary>
		/// Бинд зависимсотей со сцены
		/// </summary>
		private void InstallSceneDependencies()
		{
			Container.BindInstance(_platformManager).AsSingle();

			Container.BindInstance(_sphereController).AsSingle();

			Container.BindInstance(_finishGamePanel).AsSingle();

			Container.BindInstance(_newRecordMessage).AsSingle();

			Container.BindInstance(_scoreView).AsSingle();
		}

		/// <summary>
		/// Бинд сигналов
		/// </summary>
		private void InstallSignals()
		{
			SignalBusInstaller.Install(Container);

			Container.DeclareSignal<GameStateSignal>();
			Container.DeclareSignal<PlatformCompleteSignal>();
		}
	}
}