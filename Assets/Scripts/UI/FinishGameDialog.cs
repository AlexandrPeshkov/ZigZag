using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag
{
	public class FinishGameDialog : MonoBehaviour
	{
		[SerializeField]
		private Button _newGameButton;

		private SignalBus _signalBus;

		private GameStateService _stateService;

		[Inject]
		private void Construct(SignalBus signalBus, GameStateService stateService)
		{
			_signalBus = signalBus;
			_stateService = stateService;

			_newGameButton.onClick.AddListener(OnNewGameClick);
			_signalBus.Subscribe<GameStateSignal>(OnGameStateChanged);
		}

		private void OnNewGameClick()
		{
			_stateService.ChangeState(GameState.Reset);
		}

		private void OnGameStateChanged(GameStateSignal signal)
		{
			switch (signal.GameState)
			{
				case GameState.Failed:
					{
						gameObject.SetActive(true);
						break;
					}
				default:
					{
						gameObject.SetActive(false);
						break;
					}
			}
		}
	}
}